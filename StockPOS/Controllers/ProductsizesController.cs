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
    public class ProductsizesController : BaseController
    {
        public ProductsizesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Productsizes
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProductsizes()
        {
            try
            {
                var Productsizelist = await _repositoryWrapper.Productsize.FindAllAsync();
                return Ok(new { status = "success", data = Productsizelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get ProductsizesList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Productsize/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productsize>> GetProductsize(int id)
        {
            var Productsize = await _repositoryWrapper.Productsize.FindByIDAsync(id);

            if (Productsize == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = Productsize });
        }

        // PUT: api/Productsize/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductsize(int id, Productsize Productsize)
        {
            if (id != Productsize.ProductSizeId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Productsize.UpdateAsync(Productsize);
                await _repositoryWrapper.Eventlog.Update(Productsize);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductsizeExists(id))
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

        // POST: api/Productsize
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Productsize>> PostProductsize(Productsize Productsize)
        {
            try
            {
                await _repositoryWrapper.Productsize.CreateAsync(Productsize);
                await _repositoryWrapper.Eventlog.Insert(Productsize);

                return CreatedAtAction(nameof(GetProductsize), new { id = Productsize.ProductSizeId }, new { status = "success", data = Productsize });
            }
            catch (DbUpdateException)
            {
                if (await ProductsizeExists(Productsize.ProductSizeId))
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

        // DELETE: api/Productsize/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductsize(int id)
        {
            try
            {
                var Productsize = await _repositoryWrapper.Productsize.FindByIDAsync(id);
                if (Productsize == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Productsize.DeleteAsync(Productsize);
                await _repositoryWrapper.Eventlog.Delete(Productsize);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ProductsizeExists(int id)
        {
            return await _repositoryWrapper.Productsize.IsExist(id);
        }
    }
}
