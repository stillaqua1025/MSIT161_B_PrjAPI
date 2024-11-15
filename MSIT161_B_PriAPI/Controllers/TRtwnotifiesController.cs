using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]  // 確保所有操作需要通過 JWT 認證
    public class TRtwnotifiesController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;

        public TRtwnotifiesController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/TRtwnotifies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TRtwnotify>>> GetTRtwnotifies()
        {
            return await _context.TRtwnotifies.ToListAsync();
        }

        // GET: api/TRtwnotifies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TRtwnotify>> GetTRtwnotify(int id)
        {
            //var tRtwnotify = await _context.TRtwnotifies.FindAsync(id);

            //if (tRtwnotify == null)
            //{
            //    return NotFound();
            //}

            //return tRtwnotify;
            var resultJWT = _jwtService.GetfUserIDfromJWT(out int fUserId);
            if (resultJWT != null)
            {
                // 如果有錯誤，根據具體情況返回適當的 ActionResult
                if (resultJWT is UnauthorizedResult)
                {
                    return Unauthorized(); // 返回 401 未授權
                }
                else if (resultJWT is BadRequestObjectResult)
                {
                    return BadRequest(); // 返回 400 錯誤
                }
            }
            Factory f = new Factory(_context, _jwtService);
            var notify = f.getUserNotify(fUserId);
            return Ok(notify);

        }

        // PUT: api/TRtwnotifies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTRtwnotify(int id, TRtwnotify tRtwnotify)
        {
            if (id != tRtwnotify.FNotifyId)
            {
                return BadRequest();
            }

            _context.Entry(tRtwnotify).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRtwnotifyExists(id))
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

        // POST: api/TRtwnotifies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TRtwnotify>> PostTRtwnotify(int userId, int notifyTypeId)
        {
            //_context.TRtwnotifies.Add(tRtwnotify);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTRtwnotify", new { id = tRtwnotify.FNotifyId }, tRtwnotify);

            // 檢查是否已存在該用戶的這項通知
            var notification = _context.TRtwnotifies
                .FirstOrDefault(n => n.FUserId == userId && n.FNotifyTypeId == notifyTypeId);

            if (notification == null)
            {
                // 如果該通知不存在，新增一條記錄
                notification = new TRtwnotify
                {
                    FUserId = userId,
                    FNotifyTypeId = notifyTypeId,
                    FNotifyState = true,  // 啟用通知
                    FIsNotRead = true,    // 新增時設置為未讀
                };
                // 根據 FNotifyTypeId 動態生成通知內容
                switch (notifyTypeId)
                {
                    case 1: // 被購買通知
                        notification.FNotify = $"您有一筆新訂單！";
                        break;
                    case 2: // 到貨通知
                        notification.FNotify = $"您購買的商品已送達！";
                        break;
                    case 3: // 購買通知
                        notification.FNotify = $"您的訂單已成功處理！";
                        break;
                    case 4: // 優惠通知
                        notification.FNotify = $"恭喜！您已獲得優惠券。";
                        break;
                    default:
                        notification.FNotify = "這是系統通知。";
                        break;
                }

                _context.TRtwnotifies.Add(notification);
            }
            else
            {
                // 如果已存在該通知，切換通知狀態並更新內容
                notification.FNotifyState = !notification.FNotifyState;
                notification.FIsNotRead = true; // 更新時設置為未讀

                // 根據通知類型更新內容
                switch (notifyTypeId)
                {
                    case 1:
                        notification.FNotify = $"您有一筆新訂單！";
                        break;
                    case 2:
                        notification.FNotify = $"您購買的商品已送達！";
                        break;
                    case 3:
                        notification.FNotify = $"您的訂單已成功處理！";
                        break;
                    case 4:
                        notification.FNotify = $"恭喜！您已獲得優惠券。";
                        break;
                    default:
                        notification.FNotify = "這是系統通知。";
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "通知狀態已更新" });

        }
        [HttpPost("is-not-read")]
        public async Task<ActionResult<TRtwnotify>> IsNotRead(int notificationId)
        {
            var notification = _context.TRtwnotifies.FirstOrDefault(n => n.FNotifyId == notificationId);
            if (notification != null)
            {
                notification.FIsNotRead = true;  // 將通知設置為已讀
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            return NotFound();
        }

        // DELETE: api/TRtwnotifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTRtwnotify(int id)
        {
            var tRtwnotify = await _context.TRtwnotifies.FindAsync(id);
            if (tRtwnotify == null)
            {
                return NotFound();
            }

            _context.TRtwnotifies.Remove(tRtwnotify);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TRtwnotifyExists(int id)
        {
            return _context.TRtwnotifies.Any(e => e.FNotifyId == id);
        }
    }
}
