using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TRtworderDetailsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;

        public TRtworderDetailsController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/TRtworderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TRtworderDetail>>> GetTRtworderDetails()
        {
            return await _context.TRtworderDetails.ToListAsync();
        }

        // GET: api/TRtworderDetails/5
        [HttpGet("{id}")]
        public IActionResult GetTRtworderDetail(string id)
        {
            int orderid = _context.TRtworders
                .Where(o=>o.FOrderNumber == id).Select(o=>o.FOrderId).FirstOrDefault();
            Factory f = new Factory(_context, _jwtService);
            var details = f.getOrderDetail(orderid);
            return Ok(details);
        }

        // PUT: api/TRtworderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTRtworderDetail(int id, TRtworderDetail tRtworderDetail)
        {
            if (id != tRtworderDetail.FOrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(tRtworderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRtworderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TRtworderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TRtworderDetail>> PostTRtworderDetail(TRtworderDetail tRtworderDetail)
        {
            _context.TRtworderDetails.Add(tRtworderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTRtworderDetail", new { id = tRtworderDetail.FOrderDetailId }, tRtworderDetail);
        }

        // DELETE: api/TRtworderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTRtworderDetail(int id)
        {
            var tRtworderDetail = await _context.TRtworderDetails.FindAsync(id);
            if (tRtworderDetail == null)
            {
                return NotFound();
            }

            _context.TRtworderDetails.Remove(tRtworderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TRtworderDetailExists(int id)
        {
            return _context.TRtworderDetails.Any(e => e.FOrderDetailId == id);
        }
    }
}
