using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using StockPOS.Util;
using StockPOS.Models;
using StockPOS.Repository;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using StockPOS.CustomTokenAuthProvider;
using System.Text;

namespace StockPOS.Controllers
{

    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        // public TokenData _tokenData = new TokenData();
        public readonly IRepositoryWrapper _repositoryWrapper;
        public IConfiguration _configuration;
        private byte[] _signingKey;
        private byte[] _tokenencKey;
        private TokenProviderOptions _options;
        public readonly IMapper _mapper;

        public TokenData _tokenData = new TokenData();

        public BaseController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration,IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _configuration = configuration;
            _mapper = mapper;
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
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            setDefaultDataFromToken();
        }

        private void setDefaultDataFromToken()
        {
            try
            {
                string access_token = "";
                var hdtoken = Request.Headers["Authorization"];
                if (hdtoken.Count > 0)
                {
                    access_token = hdtoken[0];
                    access_token = access_token.Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    //var jwtSecurityToken = handler.ReadJwtToken(access_token);

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
                    if (tokenJS.SignatureAlgorithm != "A256KW")   //only allow HS256 alg --> change to new encryption alg A256KW
                        throw new Exception("Invalid Algorithm " + tokenJS.SignatureAlgorithm);

                    _tokenData = Globalfunction.GetTokenData(tokenJS);
                    //var tokenS = handler.ReadToken(access_token) as JwtSecurityToken;
                    //_tokenData = Globalfunction.GetTokenData(jwtSecurityToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //await _repositoryWrapper.Eventlog.Error("Read token error", ex.Message);
            }
        }

        // public async Task setDefaultDataFromToken()
        // {
        //     try
        //     {
        //         string access_token = "";
        //         var hdtoken = Request.Headers["Authorization"];
        //         if (hdtoken.Count > 0)
        //         {
        //             access_token = hdtoken[0];
        //             access_token = access_token.Replace("Bearer ", "");
        //             var handler = new JwtSecurityTokenHandler();
        //             var tokenS = handler.ReadToken(access_token) as JwtSecurityToken;
        //             _tokenData = Globalfunction.GetTokenData(tokenS);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         await _repositoryWrapper.EventLog.Error("Read token error", ex.Message, "Base >> setDefaultDataFromToken");
        //     }
        // }   
    }
}