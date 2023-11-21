using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockPOS.Models;
using StockPOS.Models.CreateModels;
using StockPOS.Repository;
using StockPOS.Util;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        public ProductController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration, IMapper mapper) : base(repositoryWrapper, configuration, mapper)
        {
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetProducts()
        {
            try
            {
                var Productlist = await _repositoryWrapper.Product.FindAllAsync();
                return Ok(new { status = "success", data = Productlist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Productlist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Product/5
        [HttpGet("{barcode}")]
        public async Task<ActionResult<Product>> GetProduct(string barcode)
        {
            var Product = await _repositoryWrapper.Product.FindByIDAsync(barcode);

            if (Product == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Product });
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct(ProductUpdateModel product)
        {
           
            try
            {
                if (await _repositoryWrapper.Product.AnyByConditionAsync(e => e.Name == product.Name && e.ProductId != product.ProductId))
                {
                    await _repositoryWrapper.Eventlog.Warning("Product with Name: " + product.Name + " already exists");

                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Product with Name: " + product.Name+ " already exists" });
                }

                Product NewProduct = await _repositoryWrapper.Product.FindByIDAsync(product.ProductId);
                if (NewProduct != null)
                {
                    _mapper.Map(product, NewProduct);

                    NewProduct.UpdatedBy = _tokenData.UserID;
                    NewProduct.UpdatedDate = DateTime.Now;

                    await _repositoryWrapper.Product.UpdateAsync(NewProduct);
                    await _repositoryWrapper.Eventlog.Update(NewProduct);

                    return Ok(new { status = "success", data = "Updated" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, new { status = "fail", data = "Product Not Found" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.ProductId))
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

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateModel product)
        { 
            try
            {
                if (await _repositoryWrapper.Product.AnyByConditionAsync(e => e.Name == product.Name))
                {
                    await _repositoryWrapper.Eventlog.Warning("Product with Name: " + product.Name + " already exists");

                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Product with Name: " + product.Name + " already exists" });
                }


                var newObj = _mapper.Map<Product>(product);

                newObj.ProductId = GeneralUtility.GenerateGuid;
                newObj.CreatedDate = DateTime.Now;
                newObj.CreatedBy = _tokenData.UserID;
                newObj.IsVisible = 1;

                await _repositoryWrapper.Product.CreateAsync(newObj, true); //add user to database
                await _repositoryWrapper.Eventlog.Insert(newObj);

                return StatusCode(StatusCodes.Status201Created, new { status = "success", data = new { newObj } });
            }
            catch (DbUpdateException)
            {
                if (await ProductExists(product.Barcode))
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

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var Product = await _repositoryWrapper.Product.FindByIDAsync(id);
                if (Product == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Product.DeleteAsync(Product);
                await _repositoryWrapper.Eventlog.Delete(Product);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> ProductExists(string barcode)
        {
            return await _repositoryWrapper.Product.IsExist(barcode);
        }
    }
}
