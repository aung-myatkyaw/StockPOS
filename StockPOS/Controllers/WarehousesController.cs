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
    public class WarehousesController : BaseController
    {
        public WarehousesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration, IMapper mapper) : base(repositoryWrapper, configuration, mapper)
        {
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetWarehouses()
        {
            try
            {
                var WarehouseList = await _repositoryWrapper.Warehouse.FindAllAsync();
                return Ok(new { status = "success", data = WarehouseList });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get WarehouseList fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _repositoryWrapper.Warehouse.FindByIDAsync(id);

            if (warehouse == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = warehouse });
        }

        // PUT: api/Warehouses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.WareHouseInTranId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Warehouse.UpdateAsync(warehouse);
                await _repositoryWrapper.Eventlog.Update(warehouse);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WarehouseExists(id))
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

        // POST: api/Warehouses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
        {
            try
            {
                await _repositoryWrapper.Warehouse.CreateAsync(warehouse);
                await _repositoryWrapper.Eventlog.Insert(warehouse);

                return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.WareHouseInTranId }, new { status = "success", data = warehouse });
            }
            catch (DbUpdateException)
            {
                if (await WarehouseExists(warehouse.WareHouseInTranId))
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

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            try
            {
                var warehouse = await _repositoryWrapper.Warehouse.FindByIDAsync(id);
                if (warehouse == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Warehouse.DeleteAsync(warehouse);
                await _repositoryWrapper.Eventlog.Delete(warehouse);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> WarehouseExists(int id)
        {
            return await _repositoryWrapper.Warehouse.IsExist(id);
        }
    }
}
