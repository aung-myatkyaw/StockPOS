using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockPOS.Models;
using StockPOS.Repository;
using StockPOS.Models.CreateModels;
using StockPOS.Util;
using StockPOS.Models.ViewModels;
namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductcategoryController : BaseController
    {
        public ProductcategoryController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration,IMapper mapper) : base(repositoryWrapper, configuration,mapper)
        {

        }

        // GET: api/Productcategory
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProductcategorys()
        {
            try
            {
                var Productcategorylist = await _repositoryWrapper.Productcategory.FindAllAsync();
                return Ok(Productcategorylist.OrderByDescending(ex=>ex.CreatedDate));
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Productcategorylist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Productcategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productcategory>> GetProductcategory(string id)
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
        [HttpPut]
        public async Task<IActionResult> PutProductcategory(ProductCategoryUpdateModel productcategory)
        {
            try
            {
                if (await _repositoryWrapper.Productcategory.AnyByConditionAsync(e => e.CategoryName == productcategory.CategoryName && e.CategoryId!=productcategory.CategoryId))
                {
                    await _repositoryWrapper.Eventlog.Warning("Productcategory with CategoryName: " + productcategory.CategoryName + " already exists");

                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Productcategory with CategoryName: " + productcategory.CategoryName + " already exists" });
                }

                Productcategory Newcategory = await _repositoryWrapper.Productcategory.FindByIDAsync(productcategory.CategoryId);
                if(Newcategory != null)
                {
                    _mapper.Map(productcategory, Newcategory);

                    await _repositoryWrapper.Productcategory.UpdateAsync(Newcategory);
                    await _repositoryWrapper.Eventlog.Update(Newcategory);

                    return Ok(new { status = "success", data = "Updated" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent,new {status="fail",data="ProductCategory Not Found"});
                }

                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductcategoryExists(productcategory.CategoryId))
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
        public async Task<ActionResult<Productcategory>> PostProductcategory(ProductCategoryCreateModel productcategory)
        {
            try
            {
                if (await _repositoryWrapper.Productcategory.AnyByConditionAsync(e=>e.CategoryName==productcategory.CategoryName))
                {
                    await _repositoryWrapper.Eventlog.Warning("Productcategory with CategoryName: " + productcategory.CategoryName + " already exists");

                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Productcategory with CategoryName: " + productcategory.CategoryName + " already exists" });
                }

                var newObj = _mapper.Map<Productcategory>(productcategory);

                newObj.CategoryId = GeneralUtility.GenerateGuid;
                newObj.CreatedDate = DateTime.Now;
                newObj.CreatedBy = _tokenData.UserID;
                newObj.Visible = 1;
                await _repositoryWrapper.Productcategory.CreateAsync(newObj, true); //add user to database
                await _repositoryWrapper.Eventlog.Insert(newObj);

                return StatusCode(StatusCodes.Status201Created, newObj);
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Registration Failed", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Something went wrong" });
            }
           
        }

        // DELETE: api/Productcategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductcategory(string id)
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

                return Ok();
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
