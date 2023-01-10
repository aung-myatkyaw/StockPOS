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
    public class VillagesController : BaseController
    {
        public VillagesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Villages
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetVillages()
        {
            try
            {
                var Villagelist = await _repositoryWrapper.Village.FindAllAsync();
                return Ok(new { status = "success", data = Villagelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get VillagesList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Villages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Village>> GetVillage(int id)
        {
            var village = await _repositoryWrapper.Village.FindByIDAsync(id);

            if (village == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = village });
        }

        // PUT: api/Villages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVillage(int id, Village village)
        {
            if (id != village.VillageId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Village.UpdateAsync(village);
                await _repositoryWrapper.Eventlog.Update(village);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VillageExits(id))
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

        // POST: api/Villages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Village>> PostVillage(Village village)
        {
            try
            {
                await _repositoryWrapper.Village.CreateAsync(village);
                await _repositoryWrapper.Eventlog.Insert(village);

                return CreatedAtAction(nameof(GetVillage), new { id = village.VillageId }, new { status = "success", data = village });
            }
            catch (DbUpdateException)
            {
                if (await VillageExits(village.VillageId))
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

        // DELETE: api/Villages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVillage(int id)
        {
            try
            {
                var village = await _repositoryWrapper.Village.FindByIDAsync(id);
                if (village == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Village.DeleteAsync(village);
                await _repositoryWrapper.Eventlog.Delete(village);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }


            private async Task<bool> VillageExits(int id)
            {
                return await _repositoryWrapper.Village.IsExist(id);
            }
    }
}
