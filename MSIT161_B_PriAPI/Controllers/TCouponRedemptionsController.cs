using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Hubs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TCouponRedemptionsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly NotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TCouponRedemptionsController(dbMSTI161_B_ProjectContext context, NotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        // GET: api/TCouponRedemptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCouponRedemption>>> GetTCouponRedemptions()
        {
            return await _context.TCouponRedemptions.ToListAsync();
        }

        // GET: api/TCouponRedemptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TCouponRedemption>> GetTCouponRedemption(int id,int fCouponCodeid)
        {
          
            var tCouponRedemption = await _context.TCouponRedemptions.Where(c => c.FUserId == 1 && c.FCouponCodeId == fCouponCodeid).FirstOrDefaultAsync();

            if (tCouponRedemption == null)
            {
                return NotFound();
            }

            return tCouponRedemption;
        }

        // PUT: api/TCouponRedemptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTCouponRedemption(int id, [FromQuery] int fCouponCodeId, TCouponRedemptionsDTO tCouponRedemption)
        {   //[FromQuery] int fCouponCodeId 表示 fCouponCodeId 是通過查詢參數傳遞

            //if (id != tCouponRedemption.FCouponRedemptionId)
            //{
            //    return BadRequest();
            //}

            //把參數Id變成我的userid
            var couponRedemption = await _context.TCouponRedemptions
        .Where(cr => cr.FUserId == id && cr.FCouponCodeId == fCouponCodeId)
        .FirstOrDefaultAsync();
            //從userid與codeid去找紀錄

            //couponRedemption這個是資料庫的model;tCouponRedemption是我從前端丟回來的dto
            couponRedemption.FCouponUseDate = DateTime.Now;
            couponRedemption.FCouponUseState = tCouponRedemption.FCouponUseState;
            couponRedemption.FCouponUsageCount = tCouponRedemption.FCouponUsageCount;


            _context.Entry(couponRedemption).State = EntityState.Modified;
            //告訴資料庫couponRedemption有去做更動

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCouponRedemptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(couponRedemption);
        }

        // POST: api/TCouponRedemptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		public async Task<ActionResult<TCouponRedemption>> PostTCouponRedemption(TCouponRedemptionsDTO tCouponRedemptionsDto)
		{

            //var resultJWT = _jwtService.GetfUserIDfromJWT(out int fUserId);
            //if (resultJWT != null)
            //{
            //    // 如果有錯誤，根據具體情況返回適當的 ActionResult
            //    if (resultJWT is UnauthorizedResult)
            //    {
            //        return Unauthorized(); // 返回 401 未授權
            //    }
            //    else if (resultJWT is BadRequestObjectResult)
            //    {
            //        return BadRequest(); // 返回 400 錯誤
            //    }
            //}

            var repeatCoupon= await _context.TCouponRedemptions
            .Where(c => c.FCouponCodeId == tCouponRedemptionsDto.FCouponCodeId && c.FUserId ==tCouponRedemptionsDto.FUserId)
            .FirstOrDefaultAsync();

            if (repeatCoupon != null)
            {
                return BadRequest("已經領過此優惠券"); // 提示用戶已經領取過
            }


            // 將 DTO 轉換為實體類別 TCouponRedemption
            var tCouponRedemption = new TCouponRedemption
			{
				FCouponCodeId = tCouponRedemptionsDto.FCouponCodeId,
				FUserId =1,
				FCouponGetDate = tCouponRedemptionsDto.FCouponGetDate.Value.AddHours(8),
				FCouponUseDate = tCouponRedemptionsDto.FCouponUseDate,
				FCouponUseState = tCouponRedemptionsDto.FCouponUseState,
				FCouponUsageCount = tCouponRedemptionsDto.FCouponUsageCount
			};

			// 將資料新增到資料庫
			_context.TCouponRedemptions.Add(tCouponRedemption);
			await _context.SaveChangesAsync();

            // 通知用戶
            await _notificationService.NotifyUser(1, 4); // 4 表示優惠通知

            //var message = "優惠券成功領取";
            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);


            return CreatedAtAction("GetTCouponRedemption", new { id = tCouponRedemption.FCouponRedemptionId }, tCouponRedemption);
		}

		// DELETE: api/TCouponRedemptions/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTCouponRedemption(int id)
        {
            var tCouponRedemption = await _context.TCouponRedemptions.FindAsync(id);
            if (tCouponRedemption == null)
            {
                return NotFound();
            }

            _context.TCouponRedemptions.Remove(tCouponRedemption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TCouponRedemptionExists(int id)
        {
            return _context.TCouponRedemptions.Any(e => e.FCouponRedemptionId == id);
        }
    }
}
