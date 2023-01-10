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
    public class ProducttypesController : BaseController
    {
        public ProducttypesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/GetProducttypes
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProducttypes()
        {
            try
            {
                var Producttypelist = await _repositoryWrapper.Producttype.FindAllAsync();
                return Ok(new { status = "success", data = Producttypelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get ProducttypesList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Producttypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producttype>> GetProducttype(int id)
        {
            var Producttype = await _repositoryWrapper.Producttype.FindByIDAsync(id);

            if (Producttype == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = Producttype });
        }

        // PUT: api/Producttypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducttype(string id, Producttype producttype)
        {
            if (id != producttype.ProductTypeId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Producttype.UpdateAsync(producttype);
                await _repositoryWrapper.Eventlog.Update(producttype);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProducttypeExists(id))
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

        // POST: api/Producttypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producttype>> PostProducttype(Producttype producttype)
        {
            try
            {
                await _repositoryWrapper.Producttype.CreateAsync(producttype);
                await _repositoryWrapper.Eventlog.Insert(producttype);

                return CreatedAtAction(nameof(GetProducttype), new { id = producttype.ProductTypeId }, new { status = "success", data = producttype });
            }
            catch (DbUpdateException)
            {
                if (await ProducttypeExists(producttype.ProductTypeId))
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

        // DELETE: api/Producttypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducttype(int id)
        {
            try
            {
                var Producttype = await _repositoryWrapper.Producttype.FindByIDAsync(id);
                if (Producttype == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Producttype.DeleteAsync(Producttype);
                await _repositoryWrapper.Eventlog.Delete(Producttype);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ProducttypeExists(string id)
        {
            return await _repositoryWrapper.Producttype.IsExist(id);
        }
    }
}
