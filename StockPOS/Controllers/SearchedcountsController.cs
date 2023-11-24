using System;
using System.Collections.Generic;
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
    public class SearchedcountsController : BaseController
    {
        public SearchedcountsController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration, IMapper mapper) : base(repositoryWrapper, configuration, mapper)
        {
        }

        // GET: api/ Searchedcounts
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetSearchedcounts()
        {
            try
            {
                var Searchedcountlist = await _repositoryWrapper.Searchedcount.FindAllAsync();
                return Ok(new { status = "success", data = Searchedcountlist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get  SearchedcountList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/ Searchedcounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Searchedcount>> GetSearchedcount(int id)
        {
            var searchedcount = await _repositoryWrapper.Searchedcount.FindByIDAsync(id);

            if (searchedcount == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = searchedcount });
        }

        // PUT: api/Searchedcounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSearchedcount(int id, Searchedcount searchedcount)
        {
            if (id != searchedcount.SearchId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Searchedcount.UpdateAsync(searchedcount);
                await _repositoryWrapper.Eventlog.Update(searchedcount);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SearchedcountExists(id))
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

        // POST: api/Searchedcounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Searchedcount>> PostSearchedcount(Searchedcount searchedcount)
        {
            try
            {
                await _repositoryWrapper.Searchedcount.CreateAsync(searchedcount);
                await _repositoryWrapper.Eventlog.Insert(searchedcount);

                return CreatedAtAction(nameof(GetSearchedcount), new { id = searchedcount.SearchId }, new { status = "success", data = searchedcount });
            }
            catch (DbUpdateException)
            {
                if (await SearchedcountExists(searchedcount.SearchId))
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

        // DELETE: api/Searchedcounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchedcount(int id)
        {
            try
            {
                var Searchedcount = await _repositoryWrapper.Searchedcount.FindByIDAsync(id);
                if (Searchedcount == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Searchedcount.DeleteAsync(Searchedcount);
                await _repositoryWrapper.Eventlog.Delete(Searchedcount);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> SearchedcountExists(int id)
        {
            return await _repositoryWrapper.Searchedcount.IsExist(id);
        }
    }
}
