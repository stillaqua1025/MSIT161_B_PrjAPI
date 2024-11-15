using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.Models;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TNotifyTypesController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;

        public TNotifyTypesController(dbMSTI161_B_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/TNotifyTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TNotifyType>>> GetTNotifyTypes()
        {
            return await _context.TNotifyTypes.ToListAsync();
        }

        // GET: api/TNotifyTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TNotifyType>> GetTNotifyType(int id)
        {
            var tNotifyType = await _context.TNotifyTypes.FindAsync(id);

            if (tNotifyType == null)
            {
                return NotFound();
            }

            return tNotifyType;
        }

        // PUT: api/TNotifyTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTNotifyType(int id, TNotifyType tNotifyType)
        {
            if (id != tNotifyType.FNotifyTypeId)
            {
                return BadRequest();
            }

            _context.Entry(tNotifyType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TNotifyTypeExists(id))
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

        // POST: api/TNotifyTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TNotifyType>> PostTNotifyType(TNotifyType tNotifyType)
        {
            _context.TNotifyTypes.Add(tNotifyType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTNotifyType", new { id = tNotifyType.FNotifyTypeId }, tNotifyType);
        }

        // DELETE: api/TNotifyTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTNotifyType(int id)
        {
            var tNotifyType = await _context.TNotifyTypes.FindAsync(id);
            if (tNotifyType == null)
            {
                return NotFound();
            }

            _context.TNotifyTypes.Remove(tNotifyType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TNotifyTypeExists(int id)
        {
            return _context.TNotifyTypes.Any(e => e.FNotifyTypeId == id);
        }
    }
}
