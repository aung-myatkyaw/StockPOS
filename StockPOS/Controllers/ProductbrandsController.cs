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
    public class ProductbrandsController : BaseController
    {
        public ProductbrandsController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/ Productbrands
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProductbrands()
        {
            try
            {
                var Productbrandslist = await _repositoryWrapper.Productbrand.FindAllAsync();
                return Ok(new { status = "success", data = Productbrandslist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get  Productbrands fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/ Productbrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productbrand>> GetProductbrand(int id)
        {
            var productbrand = await _repositoryWrapper.Productbrand.FindByIDAsync(id);

            if (productbrand == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = productbrand });
        }

        // PUT: api/Productbrand/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductbrand(string id, Productbrand productbrand)
        {
            if (id != productbrand.BrandId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Productbrand.UpdateAsync(productbrand);
                await _repositoryWrapper.Eventlog.Update(productbrand);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductbrandExists(id))
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

        // POST: api/Productbrands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Productbrand>> PostProductbrand(Productbrand productbrand)
        {
            try
            {
                await _repositoryWrapper.Productbrand.CreateAsync(productbrand);
                await _repositoryWrapper.Eventlog.Insert(productbrand);

                return CreatedAtAction(nameof(GetProductbrand), new { id = productbrand.BrandId }, new { status = "success", data = productbrand });
            }
            catch (DbUpdateException)
            {
                if (await ProductbrandExists(productbrand.BrandId))
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

        // DELETE: api/Productbrands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductbrand(int id)
        {
            try
            {
                var Productbrand = await _repositoryWrapper.Productbrand.FindByIDAsync(id);
                if (Productbrand == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Productbrand.DeleteAsync(Productbrand);
                await _repositoryWrapper.Eventlog.Delete(Productbrand);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ProductbrandExists(string id)
        {
            return await _repositoryWrapper.Productbrand.IsExist(id);
        }
    }
}
