using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPOS.Models;
using StockPOS.Repository;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangedpricelogsController : BaseController
    {
        public ChangedpricelogsController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration,IMapper mapper) : base(repositoryWrapper, configuration, mapper)
        {
        }

        // GET: api/Changedpricelogs
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetChangedpricelogs()
        {
            try
            {
                var Changedpriceloglist = await _repositoryWrapper.Changedpricelog.FindAllAsync();
                return Ok(new { status = "success", data = Changedpriceloglist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get Changedpriceloglist fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Changedpricelogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Changedpricelog>> GetChangedpricelog(int id)
        {
            var Changedpricelog = await _repositoryWrapper.Changedpricelog.FindByIDAsync(id);

            if (Changedpricelog == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = Changedpricelog });
        }

        // PUT: api/Changedpricelogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChangedpricelog(int id, Changedpricelog changedpricelog)
        {
            if (id != changedpricelog.PriceLogId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Changedpricelog.UpdateAsync(changedpricelog);
                await _repositoryWrapper.Eventlog.Update(changedpricelog);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ChangedpricelogExists(id))
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

        // POST: api/Changedpricelog
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Changedpricelog>> PostChangedpricelog(Changedpricelog changedpricelog)
        {
            try
            {
                await _repositoryWrapper.Changedpricelog.CreateAsync(changedpricelog);
                await _repositoryWrapper.Eventlog.Insert(changedpricelog);

                return CreatedAtAction(nameof(GetChangedpricelog), new { id = changedpricelog.PriceLogId }, new { status = "success", data = changedpricelog });
            }
            catch (DbUpdateException)
            {
                if (await ChangedpricelogExists(changedpricelog.PriceLogId))
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

        // DELETE: api/Changedpricelog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChangedpricelog(int id)
        {
            try
            {
                var Changedpricelog = await _repositoryWrapper.Changedpricelog.FindByIDAsync(id);
                if (Changedpricelog == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Changedpricelog.DeleteAsync(Changedpricelog);
                await _repositoryWrapper.Eventlog.Delete(Changedpricelog);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ChangedpricelogExists(int id)
        {
            return await _repositoryWrapper.Changedpricelog.IsExist(id);
        }

    }
}
