using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockPOS.Models;
using StockPOS.Repository;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtController : BaseController
    {
        public BoughtController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Bought
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetBoughts()
        {
            try
            {
                var boughtlist = await _repositoryWrapper.Bought.FindAllAsync();
                return Ok(new { status = "success", data = boughtlist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get boughtlist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Bought/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bought>> GetBought(int id)
        {
            var bought = await _repositoryWrapper.Bought.FindByIDAsync(id);

            if (bought == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = bought });
        }

        // PUT: api/Bought/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBought(int id, Bought bought)
        {
            if (id != bought.Id)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Bought.UpdateAsync(bought);
                await _repositoryWrapper.Eventlog.Update(bought);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BoughtExists(id))
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters."});
                }
            }
            catch(Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("update fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // POST: api/Bought
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bought>> PostBought(Bought bought)
        { 
            try
            {
                await _repositoryWrapper.Bought.CreateAsync(bought);
                await _repositoryWrapper.Eventlog.Insert(bought);

                return CreatedAtAction(nameof(GetBought), new { id = bought.Id }, new { status = "success", data = bought });
            }
            catch (DbUpdateException)
            {
                if (await BoughtExists(bought.Id))
                {
                    return Conflict(new { status = "fail", data = "Data Already Exist."});
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters."});
                }
            }
            catch(Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("create fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // DELETE: api/Bought/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBought(int id)
        {
            try
            {
                var bought = await _repositoryWrapper.Bought.FindByIDAsync(id);
                if (bought == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Bought.DeleteAsync(bought);
                await _repositoryWrapper.Eventlog.Delete(bought);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> BoughtExists(int id)
        {
            return await _repositoryWrapper.Bought.IsExist(id);
        }
    }
}
