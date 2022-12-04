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
    public class ProductcategoryController : BaseController
    {
        public ProductcategoryController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Productcategory
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProductcategorys()
        {
            try
            {
                var Productcategorylist = await _repositoryWrapper.Productcategory.FindAllAsync();
                return Ok(new { status = "success", data = Productcategorylist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Productcategorylist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Productcategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productcategory>> GetProductcategory(int id)
        {
            var Productcategory = await _repositoryWrapper.Productcategory.FindByIDAsync(id);

            if (Productcategory == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Productcategory });
        }

        // PUT: api/Productcategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductcategory(string id, Productcategory Productcategory)
        {
            if (id != Productcategory.CategoryId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Productcategory.UpdateAsync(Productcategory);
                await _repositoryWrapper.Eventlog.Update(Productcategory);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductcategoryExists(id))
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

        // POST: api/Productcategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Productcategory>> PostProductcategory(Productcategory Productcategory)
        { 
            try
            {
                await _repositoryWrapper.Productcategory.CreateAsync(Productcategory);
                await _repositoryWrapper.Eventlog.Insert(Productcategory);

                return CreatedAtAction(nameof(GetProductcategory), new { id = Productcategory.CategoryId }, new { status = "success", data = Productcategory });
            }
            catch (DbUpdateException)
            {
                if (await ProductcategoryExists(Productcategory.CategoryId))
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

        // DELETE: api/Productcategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductcategory(int id)
        {
            try
            {
                var Productcategory = await _repositoryWrapper.Productcategory.FindByIDAsync(id);
                if (Productcategory == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Productcategory.DeleteAsync(Productcategory);
                await _repositoryWrapper.Eventlog.Delete(Productcategory);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ProductcategoryExists(string id)
        {
            return await _repositoryWrapper.Productcategory.IsExist(id);
        }
    }
}
