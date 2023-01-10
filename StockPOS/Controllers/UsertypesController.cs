using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPOS.Models;
using StockPOS.Repository;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsertypesController : BaseController
    {
        public UsertypesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Usertypes
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetUsertypes()
        {
            try
            {
                var Usertypelist = await _repositoryWrapper.Usertype.FindAllAsync();
                return Ok(new { status = "success", data = Usertypelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get UsertypeList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Usertypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usertype>> GetUsertype(int id)
        {
            var usertype = await _repositoryWrapper.Usertype.FindByIDAsync(id);

            if (usertype == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = usertype });
        }

        // PUT: api/Usertypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsertype(int id, Usertype usertype)
        {
            if (id != usertype.UserTypeId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Usertype.UpdateAsync(usertype);
                await _repositoryWrapper.Eventlog.Update(usertype);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsertypeExists(id))
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters." });
                }
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("update fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // POST: api/Usertypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usertype>> PostUsertype(Usertype usertype)
        {
            try
            {
                await _repositoryWrapper.Usertype.CreateAsync(usertype);
                await _repositoryWrapper.Eventlog.Insert(usertype);

                return CreatedAtAction(nameof(GetUsertype), new { id = usertype.UserTypeId }, new { status = "success", data = usertype });
            }
            catch (DbUpdateException)
            {
                if (await UsertypeExists(usertype.UserTypeId))
                {
                    return Conflict(new { status = "fail", data = "Data Already Exist." });
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters." });
                }
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("create fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // DELETE: api/Usertypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsertype(int id)
        {
            try
            {
                var Usertype = await _repositoryWrapper.Usertype.FindByIDAsync(id);
                if (Usertype == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Usertype.DeleteAsync(Usertype);
                await _repositoryWrapper.Eventlog.Delete(Usertype);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> UsertypeExists(int id)
        {
            return await _repositoryWrapper.Usertype.IsExist(id);
        }
    }
}
