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
    public class DebtbalancesController : BaseController
    {
        public DebtbalancesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Debtbalances
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetDebtbalances()
        {
            try
            {
                var Debtbalancelist = await _repositoryWrapper.Debtbalance.FindAllAsync();
                return Ok(new { status = "success", data = Debtbalancelist });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get Debtbalancelist fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }


        // GET: api/Debtbalances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Debtbalance>> GetDebtbalance(int id)
        {
            var debtbalance = await _repositoryWrapper.Debtbalance.FindByIDAsync(id);

            if (debtbalance == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok(new { status = "success", data = debtbalance });
        }

        // PUT: api/Debtbalance/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDebtbalance(int id, Debtbalance debtbalance)
        {
            if (id != debtbalance.DebtBalanceId)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Debtbalance.UpdateAsync(debtbalance);
                await _repositoryWrapper.Eventlog.Update(debtbalance);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DebtbalanceExists(id))
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

        // POST: api/Debtbalance
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Debtbalance>> PostDebtbalance(Debtbalance debtbalance)
        {
            try
            {
                await _repositoryWrapper.Debtbalance.CreateAsync(debtbalance);
                await _repositoryWrapper.Eventlog.Insert(debtbalance);

                return CreatedAtAction(nameof(GetDebtbalance), new { id = debtbalance.DebtBalanceId }, new { status = "success", data = debtbalance });
            }
            catch (DbUpdateException)
            {
                if (await DebtbalanceExists(debtbalance.DebtBalanceId))
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

        // DELETE: api/Debtbalances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebtbalance(int id)
        {
            try
            {
                var Debtbalance = await _repositoryWrapper.Debtbalance.FindByIDAsync(id);
                if (Debtbalance == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found." });
                }

                await _repositoryWrapper.Debtbalance.DeleteAsync(Debtbalance);
                await _repositoryWrapper.Eventlog.Delete(Debtbalance);

                return Ok(new { status = "success", data = "Deleted" });
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest(new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> DebtbalanceExists(int id)
        {
            return await _repositoryWrapper.Debtbalance.IsExist(id);
        }
    }
}
