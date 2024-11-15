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
using MSIT161_B_PriAPI.Repositories;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // 確保所有操作需要通過 JWT 認證
    public class TCouponsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
		private readonly JwtService _jwtService;

		public TCouponsController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
			_jwtService = jwtService;
		}

        // GET: api/TCoupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCoupon>>> GetTCoupons()
        {
            return await _context.TCoupons.ToListAsync();
        }

        // GET: api/TCoupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TCoupon>> GetTCoupon(int id, [FromQuery] int? fCoupon_from)
        {   //抓取目前使用者有哪些優惠券
			//假设你有一个 API 路由是 GET /api/TCoupons/{id}，并且你希望通过查询参数进行进一步的筛选，
			//比如根据创建者 fCoupon_from 来筛选优惠券。
			//你可以使用 [FromQuery] 属性从查询字符串中获取 fCoupon_from 的值。


			//var tCoupon = await _context.TCoupons.FindAsync(id);

			//if (tCoupon == null)
			//{
			//    return NotFound();
			//}

			//return tCoupon;


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
			Factory f = new Factory(_context, _jwtService);
            var coupons = f.getCoupons(fUserId, fCoupon_from);
            return Ok(coupons);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<TCoupon>> GetSellerTCoupon(int couponfromID)
        //{
        //    //假设你有一个 API 路由是 GET /api/TCoupons/{id}，并且你希望通过查询参数进行进一步的筛选，
        //    //比如根据创建者 fCoupon_from 来筛选优惠券。
        //    //你可以使用 [FromQuery] 属性从查询字符串中获取 fCoupon_from 的值。


        //    //var tCoupon = await _context.TCoupons.FindAsync(id);

        //    //if (tCoupon == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return tCoupon;
        //    Factory f = new Factory(_context);
        //    var coupons = f.getSellerCoupons(couponfromID);
        //    return Ok(coupons);
        //}

        // PUT: api/TCoupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTCoupon(int id, TCoupon tCoupon)
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

			if (id != tCoupon.FCouponCodeId)
            {
                return BadRequest();
            }

            _context.Entry(tCoupon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCouponExists(id))
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

        // POST: api/TCoupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TCoupon>> PostTCoupon(CouponDTO couponDTO)
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

			var tCoupon = new TCoupon
			{
				FCouponCodeName = couponDTO.FCouponCodeName,
				FCouponCode = couponDTO.FCouponCode,
				FCouponDiscount = couponDTO.FCouponDiscount,
				FCouponCreatday = couponDTO.FCouponCreatday,
				FCouponEndday = couponDTO.FCouponEndday,
				FCouponFrom = fUserId,
				CouponDescription = couponDTO.CouponDescription,
				MinSellMoney = couponDTO.MinSellMoney
			};

			_context.TCoupons.Add(tCoupon);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTCoupon", new { id = tCoupon.FCouponCodeId }, tCoupon);

        }

        // DELETE: api/TCoupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTCoupon(int id)
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

			var tCoupon = await _context.TCoupons.FindAsync(id);
            if (tCoupon == null)
            {
                return NotFound();
            }

            _context.TCoupons.Remove(tCoupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TCouponExists(int id)
        {
            return _context.TCoupons.Any(e => e.FCouponCodeId == id);
        }
    }
}
