using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockPOS.Models;
using StockPOS.Repository;

namespace StockPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : BaseController
    {
        public SaleController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration, IMapper mapper) : base(repositoryWrapper, configuration, mapper)
        {
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetSales()
        {
            try
            {
                var Salelist = await _repositoryWrapper.Sale.FindAllAsync();
                return Ok(new { status = "success", data = Salelist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Salelist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var Sale = await _repositoryWrapper.Sale.FindByIDAsync(id);

            if (Sale == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Sale });
        }

        // PUT: api/Sale/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale Sale)
        {
            if (id != Sale.SaleId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Sale.UpdateAsync(Sale);
                await _repositoryWrapper.Eventlog.Update(Sale);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SaleExists(id))
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

        // POST: api/Sale
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale Sale)
        { 
            try
            {
                await _repositoryWrapper.Sale.CreateAsync(Sale);
                await _repositoryWrapper.Eventlog.Insert(Sale);

                return CreatedAtAction(nameof(GetSale), new { id = Sale.SaleId }, new { status = "success", data = Sale });
            }
            catch (DbUpdateException)
            {
                if (await SaleExists(Sale.SaleId))
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

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            try
            {
                var Sale = await _repositoryWrapper.Sale.FindByIDAsync(id);
                if (Sale == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Sale.DeleteAsync(Sale);
                await _repositoryWrapper.Eventlog.Delete(Sale);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> SaleExists(int id)
        {
            return await _repositoryWrapper.Sale.IsExist(id);
        }
    }
}
