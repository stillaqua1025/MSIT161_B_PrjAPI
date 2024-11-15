using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // 確保所有操作需要通過 JWT 認證
    public class TRtwcommentsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;

        public TRtwcommentsController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/TRtwcomments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TRtwcomment>>> GetTRtwcomments()
        {
            return await _context.TRtwcomments.ToListAsync();
        }

        // GET: api/TRtwcomments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TRtwcomment>> GetTRtwcomment(int id)
        {
            //var tRtwcomment = await _context.TRtwcomments.FindAsync(id);

            //if (tRtwcomment == null)
            //{
            //    return NotFound();
            //}

            var comment = _context.TRtwcomments.Join(_context.TRtwproducts, c => c.FSellerId, p => p.FUserId, (c, p) => new {c,p}).Where(c => c.c.FSellerId == id).
                Select(c => new SellerCommentDTO
                {
                    FCommentId = c.c.FCommentId,
                    FComment = c.c.FComment,
                    FSellerid = c.p.FUserId,
                    FUserName = c.c.FUser.FName,
                    FCommentDate = c.c.FCommentDate,
                    FScore = c.c.FScore,
                    AnonymousUser = c.c.AnonymousUser,
                    

                }).Distinct();
            //JOIN 操作經常會導致數據重複（如果表之間有一對多關係），因為每次聯結都會生成多行數據。
            return Ok(comment);
        }

        // PUT: api/TRtwcomments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTRtwcomment(int id, TRtwcomment tRtwcomment)
        {
            if (id != tRtwcomment.FCommentId)
            {
                return BadRequest();
            }

            _context.Entry(tRtwcomment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRtwcommentExists(id))
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

        // POST: api/TRtwcomments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TRtwcomment>> PostTRtwcomment(CreatCommentDTO CreatComment)
        {

            // 使用 JwtService 來獲取 fUserId
            var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
            if (result != null)
            {
                // 如果有錯誤，根據具體情況返回適當的 ActionResult
                if (result is UnauthorizedResult)
                {
                    return Unauthorized(); // 返回 401 未授權
                }
                else if (result is BadRequestObjectResult)
                {
                    return BadRequest(); // 返回 400 錯誤
                }
            }
            DateOnly currentday=DateOnly.FromDateTime(DateTime.Now);
            var comment = new TRtwcomment
            { 
				FSellerId = CreatComment.FSellerId,
                FUserId= fUserId,
                FScore=CreatComment.FScore,
                FComment=CreatComment.FComment,
                FCommentDate= currentday,
                AnonymousUser=CreatComment.AnonymousUser,
                FSellerReply=CreatComment.FSellerReply,
            };


			_context.TRtwcomments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTRtwcomment", new { id = comment.FCommentId }, comment);
        }

        // DELETE: api/TRtwcomments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTRtwcomment(int id)
        {
            var tRtwcomment = await _context.TRtwcomments.FindAsync(id);
            if (tRtwcomment == null)
            {
                return NotFound();
            }

            _context.TRtwcomments.Remove(tRtwcomment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TRtwcommentExists(int id)
        {
            return _context.TRtwcomments.Any(e => e.FCommentId == id);
        }
    }
}
