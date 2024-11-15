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
    public class TSendNotifiesController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;

        public TSendNotifiesController(dbMSTI161_B_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/TSendNotifies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSendNotify>>> GetTSendNotifies()
        {
            return await _context.TSendNotifies.ToListAsync();
        }

        // GET: api/TSendNotifies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TSendNotify>> GetTSendNotify(int id)
        {
            var tSendNotify = await _context.TSendNotifies.FindAsync(id);

            if (tSendNotify == null)
            {
                return NotFound();
            }

            return tSendNotify;
        }

        // PUT: api/TSendNotifies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTSendNotify(int id, TSendNotify tSendNotify)
        {
            if (id != tSendNotify.FSendNotifyId)
            {
                return BadRequest();
            }

            _context.Entry(tSendNotify).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TSendNotifyExists(id))
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

        // POST: api/TSendNotifies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TSendNotify>> PostTSendNotify(TSendNotify tSendNotify)
        {
            _context.TSendNotifies.Add(tSendNotify);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTSendNotify", new { id = tSendNotify.FSendNotifyId }, tSendNotify);
        }

        // DELETE: api/TSendNotifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTSendNotify(int id)
        {
            var tSendNotify = await _context.TSendNotifies.FindAsync(id);
            if (tSendNotify == null)
            {
                return NotFound();
            }

            _context.TSendNotifies.Remove(tSendNotify);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TSendNotifyExists(int id)
        {
            return _context.TSendNotifies.Any(e => e.FSendNotifyId == id);
        }
    }
}
