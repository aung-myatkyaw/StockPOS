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
    public class GroupvillagesController : BaseController
    {
        public GroupvillagesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Groupvillages
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetGroupvillages()
        {
            try
            {
                var Groupvillagelist = await _repositoryWrapper.Groupvillage.FindAllAsync();
                return Ok(new { status = "success", data = Groupvillagelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get GroupvillageList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Groupvillages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groupvillage>> GetGroupvillage(int id)
        {
            var groupvillage = await _repositoryWrapper.Groupvillage.FindByIDAsync(id);

            if (groupvillage == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = groupvillage });
        }

        // PUT: api/Groupvillages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupvillage(int id, Groupvillage groupvillage)
        {
            if (id != groupvillage.GroupVillageId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Groupvillage.UpdateAsync(groupvillage);
                await _repositoryWrapper.Eventlog.Update(groupvillage);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GroupvillageExists(id))
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

        // POST: api/Groupvillages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Groupvillage>> PostGroupvillage(Groupvillage groupvillage)
        {
            try
            {
                await _repositoryWrapper.Groupvillage.CreateAsync(groupvillage);
                await _repositoryWrapper.Eventlog.Insert(groupvillage);

                return CreatedAtAction(nameof(GetGroupvillage), new { id = groupvillage.GroupVillageId }, new { status = "success", data = groupvillage });
            }
            catch (DbUpdateException)
            {
                if (await GroupvillageExists(groupvillage.GroupVillageId))
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

        // DELETE: api/Groupvillages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupvillage(int id)
        {
            try
            {
                var Groupvillage = await _repositoryWrapper.Groupvillage.FindByIDAsync(id);
                if (Groupvillage == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Groupvillage.DeleteAsync(Groupvillage);
                await _repositoryWrapper.Eventlog.Delete(Groupvillage);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> GroupvillageExists(int id)
        {
            return await _repositoryWrapper.Groupvillage.IsExist(id);
        }
    }
}
