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
    public class CashierController : BaseController
    {
        public CashierController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Cashier
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetCashiers()
        {
            try
            {
                var Cashierlist = (await _repositoryWrapper.Cashier.FindAllAsync()).Select(item => item.AsDTO());
                return Ok(new { status = "success", data = Cashierlist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Cashierlist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Cashier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CashierDTO>> GetCashier(int id)
        {
            var Cashier = await _repositoryWrapper.Cashier.FindByIDAsync(id);

            if (Cashier == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Cashier.AsDTO() });
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<dynamic>> RegisterCashier(CashierRegisterDTO cashierDTO)
        {
            try
            {
                //search user for duplicate username
                bool check = await _repositoryWrapper.Cashier.CheckExistingUserName(cashierDTO.UserName);
                if (check)
                {
                    await _repositoryWrapper.Eventlog.Warning("Cashier with UserName: " + cashierDTO.UserName + " already exists");
                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Cashier with UserName: " + cashierDTO.UserName + " already exists" });
                }

                //create new cashier
                string salt = Util.SaltedHash.GenerateSalt();
                Cashier newObj = new Cashier()
                {
                    FullName = cashierDTO.FullName,
                    Phone = cashierDTO.Phone,
                    UserName = cashierDTO.UserName,
                    DateofBirth = cashierDTO.DateofBirth,
                    DateCreated = DateTime.UtcNow,
                    Gender = cashierDTO.Gender,
                    Address = cashierDTO.Address,
                    Email = cashierDTO.Email,
                    PasswordSalt = salt,
                    Password = Util.SaltedHash.ComputeHash(salt, cashierDTO.Password)
                };

                await _repositoryWrapper.Cashier.CreateAsync(newObj, true); //add user to database
                await _repositoryWrapper.Eventlog.Insert(newObj);

                return StatusCode(StatusCodes.Status201Created, new { status = "success", data = new { newObj.Id, newObj.UserName }});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Registration Failed", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Something went wrong" });
            }
        }

        // DELETE: api/Cashier/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashier(int id)
        {
            try
            {
                var Cashier = await _repositoryWrapper.Cashier.FindByIDAsync(id);
                if (Cashier == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Cashier.DeleteAsync(Cashier);
                await _repositoryWrapper.Eventlog.Delete(Cashier);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> CashierExists(int id)
        {
            return await _repositoryWrapper.Cashier.IsExist(id);
        }
    }
}
