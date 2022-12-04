using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockPOS.Models;
using StockPOS.Repository;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetUsers()
        {
            try
            {
                var Userlist = (await _repositoryWrapper.User.FindAllAsync()).Select(item => item.AsDTO());
                return Ok(new { status = "success", data = Userlist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Userlist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var User = await _repositoryWrapper.User.FindByIDAsync(id);

            if (User == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = User.AsDTO() });
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<dynamic>> RegisterUser(UserRegisterDTO userDTO)
        {
            try
            {
                //search user for duplicate username
                bool check = await _repositoryWrapper.User.CheckExistingUserName(userDTO.UserName);
                if (check)
                {
                    await _repositoryWrapper.Eventlog.Warning("User with UserName: " + userDTO.UserName + " already exists");
                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "User with UserName: " + userDTO.UserName + " already exists" });
                }

                //create new user
                string salt = Util.SaltedHash.GenerateSalt();
                User newObj = new()
                {
                    FullName = userDTO.FullName,
                    Phone = userDTO.Phone,
                    UserName = userDTO.UserName,
                    DateofBirth = userDTO.DateofBirth,
                    DateCreated = DateTime.UtcNow,
                    Gender = userDTO.Gender,
                    Address = userDTO.Address,
                    Email = userDTO.Email,
                    PasswordSalt = salt,
                    Password = Util.SaltedHash.ComputeHash(salt, userDTO.Password),
                    UserTypeId = userDTO.UserTypeId
                };

                await _repositoryWrapper.User.CreateAsync(newObj, true); //add user to database
                await _repositoryWrapper.Eventlog.Insert(newObj);

                return StatusCode(StatusCodes.Status201Created, new { status = "success", data = new { newObj.UserId, newObj.UserName }});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Registration Failed", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Something went wrong" });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var User = await _repositoryWrapper.User.FindByIDAsync(id);
                if (User == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.User.DeleteAsync(User);
                await _repositoryWrapper.Eventlog.Delete(User);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> UserExists(int id)
        {
            return await _repositoryWrapper.User.IsExist(id);
        }
    }
}
