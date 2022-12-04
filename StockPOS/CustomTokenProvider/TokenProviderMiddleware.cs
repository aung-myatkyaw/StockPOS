using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using StockPOS.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using StockPOS.Models;
using StockPOS.Util;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StockPOS.CustomTokenAuthProvider
{
    public class TokenProviderMiddleware : IMiddleware
    {
        private IRepositoryWrapper _repository;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        private Byte[] _signingKey;
        private Byte[] _tokenencKey;

        public TokenProviderMiddleware(IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repository, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _configuration = configuration;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            double expiretimespan = Convert.ToDouble(_configuration.GetSection("TokenAuthentication:TokenExpire").Value);
            TimeSpan expiration = TimeSpan.FromMinutes(expiretimespan);
            _signingKey = Encoding.ASCII.GetBytes(_configuration.GetSection("TokenAuthentication:SecretKey").Value);
            _tokenencKey = Encoding.ASCII.GetBytes(_configuration.GetSection("TokenAuthentication:TokenEncryptionKey").Value);
            _options = new TokenProviderOptions
            {
                Path = _configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = _configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = _configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_signingKey), SecurityAlgorithms.HmacSha256),
                Expiration = expiration
            };
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            TokenData _tokenData = new TokenData();
            var access_token = string.Empty;

            if (context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                if(context.Request.Method == HttpMethods.Post) 
                {
                    await GenerateToken(context); // login with api/token
                    return;
                }
                else {  //call token endpoint with non-post http method 
                    await ResponseMessage(new { status = "fail", data = "Method Not Allowed" }, context, StatusCodes.Status405MethodNotAllowed);
                    return;
                }
            }
            else if(context.Request.Path.Equals("/error")) //default Error handler page
            {
                await ResponseMessage(new { status = "fail", data = "Something went wrong. Try again later." }, context, StatusCodes.Status500InternalServerError);
                return;
            }

            //Check Token
            var hdtoken = context.Request.Headers["Authorization"];
            if (hdtoken.Count > 0)
            {
                try 
                {
                    access_token = hdtoken[0];
                    access_token = access_token.Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    handler.ValidateToken(access_token, new TokenValidationParameters  
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_signingKey),
                        RequireSignedTokens = true,
                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = _options.Issuer,
                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = _options.Audience,
                        // Validate the token expiry
                        ValidateLifetime = true,
                        // If you want to allow a certain amount of clock drift, set that here:
                        ClockSkew = TimeSpan.Zero,
                        TokenDecryptionKey = new SymmetricSecurityKey(_tokenencKey)
                    }, out SecurityToken tokenS);
                    
                    var tokenJS = (JwtSecurityToken)tokenS;
                    if(tokenJS.SignatureAlgorithm != "A256KW")   //only allow HS256 alg --> change to new encryption alg A256KW
                        throw new Exception("Invalid Algorithm " + tokenJS.SignatureAlgorithm);

                    _tokenData = Globalfunction.GetTokenData(tokenJS);
                }
                catch(SecurityTokenExpiredException) 
                {
                    await ResponseMessage(new { status = "fail", data = "The Token has expired" }, context, StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (Exception tex)  
                {   
                    //if something change in token or fake token, got error
                    await _repository.Eventlog.Error("Invalid Token, Access Denied.", tex.Message);
                    await ResponseMessage(new { status = "fail", data = "Invalid Token, Access Denied." }, context, StatusCodes.Status401Unauthorized);
                    return;
                }
            }

            if (context.Request.Path.Equals("/", StringComparison.Ordinal) || context.Request.Path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase))
            {
                //default landing page
                await next(context);
            }
            else
            {
                string strPath = context.Request.Path.ToString();
                
                // you can add with || multiple publically available functions to skip login
                if (strPath.StartsWith("swagger") == true || strPath.Equals("/api/user/registration", StringComparison.OrdinalIgnoreCase))
                {
                    await next(context);
                }
                else if (string.IsNullOrEmpty(access_token))
                {
                    await _repository.Eventlog.Warning("Token not found");
                    await ResponseMessage(new { status = "fail", data = "Token not found" }, context, StatusCodes.Status401Unauthorized);
                }
                else
                {
                    try
                    {
                        // check token expired   
                        double expireTime = Convert.ToDouble(_options.Expiration.TotalMinutes);
                        DateTime issueDate = _tokenData.TicketExpireDate.AddMinutes(-expireTime);
                        DateTime NowDate = DateTime.UtcNow;
                        if (issueDate > NowDate || _tokenData.TicketExpireDate < NowDate)
                        {
                            // throw new Exception("Invalid Token Expire, Issue Date: " + issueDate.ToString());
                            await _repository.Eventlog.Warning("The Token has expired");
                            await ResponseMessage(new { status = "fail", data = "The Token has expired" }, context, StatusCodes.Status401Unauthorized);
                            return;
                        }
                        
                        // end of token expired check

                        var now = DateTime.UtcNow;
                        _tokenData.TicketExpireDate = now.Add(_options.Expiration);
                        var claims = Globalfunction.GetClaims(_tokenData);

                        var appIdentity = new ClaimsIdentity(claims);
                        context.User.AddIdentity(appIdentity); //add custom identity because default identity has delay to get data in EventLogRepository

                        //Regenerate newtoken for not timeout at running
                        // Create the JWT and write it to a string
                        string newToken = CreateEncryptedJWTToken(claims);

                        if (!string.IsNullOrEmpty(newToken))
                        {
                            context.Response.Headers.Add("Access-Control-Expose-Headers", "NewToken");
                            context.Response.Headers.Add("NewToken", newToken);
                            await next(context);
                        }
                    }
                    catch (Exception ex)
                    {
                        Globalfunction.WriteSystemLog("InvokeAsync: " + ex.Message);
                        await _repository.Eventlog.Error("New Token Generation Failed", ex.Message);
                        await ResponseMessage(new { status = "fail", data = "Something went wrong" }, context, StatusCodes.Status401Unauthorized);
                        return;
                    }
                }
            }            
        }

        private async Task GenerateToken(HttpContext context)
        {
            string username = string.Empty;
            string password = string.Empty;
            try
            {
                using var reader = new StreamReader(context.Request.Body);
                var request_body = reader.ReadToEnd();
                UserLoginDTO userData = JsonConvert.DeserializeObject<UserLoginDTO>(request_body, _serializerSettings);
                if (string.IsNullOrEmpty(userData.UserName) || string.IsNullOrEmpty(userData.Password))
                {
                    await _repository.Eventlog.Error("Invalid login credentials", "UserName:" + userData.UserName + ", Password: " + userData.Password);
                    await ResponseMessage(new { status = "fail", data = "Invalid login credentials" }, context, StatusCodes.Status422UnprocessableEntity);
                    return;
                }
                // phoneno = Encryption.DecryptClient_String(userData.PhoneNumber);
                // password = Encryption.DecryptClient_String(userData.Password);
                username = userData.UserName;
                password = userData.Password;
            }
            catch (Exception ex)
            {
                await _repository.Eventlog.Error("Failed to read login credentials", ex.Message);
                Globalfunction.WriteSystemLog("GenerateToken: " + ex.Message);
                await ResponseMessage(new { status = "fail", data = "Invalid login credentials" }, context, StatusCodes.Status400BadRequest);
                return;
            }

            try 
            {
                dynamic? loginresult = null;
                int UserID;
                string UserName;

                loginresult = await dologinValidation(username, password);
                if(loginresult.error == 0)
                {
                    loginresult = loginresult.data;
                    UserID = loginresult.UserId;
                    UserName = loginresult.UserName;
                }
                else 
                {
                    string error_msg = loginresult.message.ToString();
                    await ResponseMessage(new { status = "fail", data = error_msg }, context, StatusCodes.Status401Unauthorized);
                    return;
                }

                var now = DateTime.UtcNow;
                var _tokenData = new TokenData
                {
                    Sub = UserName,
                    Jti = await _options.NonceGenerator(),
                    Iat = new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                    UserID = UserID.ToString(),
                    UserName = UserName,
                    TicketExpireDate = now.Add(_options.Expiration)
                };
                var claims = Globalfunction.GetClaims(_tokenData);

                var appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);

                string encodedJwt = CreateEncryptedJWTToken(claims);

                var tokeninfo = new
                {
                    AccessToken = encodedJwt,
                    ExpiresInSeconds = (int)_options.Expiration.TotalSeconds,
                    UserId = UserID,
                    UserName
                };

                var response = new
                {
                    status = "success",
                    data = tokeninfo
                };
                
                await ResponseMessage(response, context, StatusCodes.Status200OK);
                 
                await _repository.Eventlog.Info("Successful login for this account UserName: " + username);
            }
            catch(Exception ex) 
            {
                Globalfunction.WriteSystemLog("Generate Token Fail: " + username + ", Error: " + ex.Message); 
                await _repository.Eventlog.Error("Generate Token Fail for " + username, ex.Message); 
                await ResponseMessage(new { status = "fail", data = "Generate Token Fail" }, context, StatusCodes.Status401Unauthorized);
                return;
            }
        }

        private async Task<dynamic> dologinValidation(string username, string password)
        {
            try 
            {
                User result = (await _repository.User.FindByConditionAsync(x => x.UserName == username)).FirstOrDefault();
                if (result == null)
                {
                    await _repository.Eventlog.Warning("User not found with UserName: " + username);
                    return new { error = 1, message = "User not found with UserName: " + username };
                }
    
                string oldhash = result.Password; 
                string oldsalt = result.PasswordSalt; 
                bool flag = SaltedHash.Verify(oldsalt, oldhash, password);
                if (flag)
                {
                    return new { error = 0, data = result };
                }
                await _repository.Eventlog.Warning("Password Validation Failed with " + username);
                return new { error = 1, message = "Password Validation Failed" };
            }
            catch(Exception ex) 
            {
                Globalfunction.WriteSystemLog("dologinValidation: " + ex.Message);
                await _repository.Eventlog.Error("Login Fail with UserName: " + username, ex.Message);
                return new { error = 1, message = "Login Failed"};
            }
        }

        public async Task ResponseMessage(object data, HttpContext context, int code = StatusCodes.Status400BadRequest)
        {
            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data, _serializerSettings));
        }

        private string CreateEncryptedJWTToken(Claim[] claims)
        {
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _options.Audience,
                Issuer = _options.Issuer,
                Subject = new ClaimsIdentity(claims),
                NotBefore = now,
                IssuedAt = Globalfunction.UnixTimeStampToDateTime(int.Parse(claims.First(claim => claim.Type == "iat").Value)),  //reuse same Iat
                Expires = now.Add(_options.Expiration),
                SigningCredentials = _options.SigningCredentials,
                EncryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(_tokenencKey), SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512)
            };
            var handler = new JwtSecurityTokenHandler();
            string encodedJwt = handler.CreateEncodedJwt(tokenDescriptor);
            return encodedJwt;
        }
    }
}