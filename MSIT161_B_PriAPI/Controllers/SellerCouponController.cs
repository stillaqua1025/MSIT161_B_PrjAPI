using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSIT161_B_PriAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]  //確保所有操作需要通過 JWT 認證
	public class SellerCouponController : ControllerBase
	{
		private readonly dbMSTI161_B_ProjectContext _context;
		private readonly JwtService _jwtService;

		public SellerCouponController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
		}
		// GET: api/<SellerCouponController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TCoupon>>> GetTCoupons()
		{
			return await _context.TCoupons.ToListAsync();
		}

		// GET api/<SellerCouponController>/5
		[HttpGet("{couponFromID}")]
		public async Task<ActionResult<TCoupon>> GetSellerTCoupon(int couponFromID, [FromQuery]SellerCouponSearchDTO searchDTO)
		{
			//假设你有一个 API 路由是 GET /api/TCoupons/{id}，并且你希望通过查询参数进行进一步的筛选，
			//比如根据创建者 fCoupon_from 来筛选优惠券。
			//你可以使用 [FromQuery] 属性从查询字符串中获取 fCoupon_from 的值。
			//[FromQuery] 主要用于 GET 请求，GET 請求的参數通常通過 URL 查詢字符串傳遞。
			//在查詢字符串中，參數以 key=value 的形式出現，多个參數之間用 & 符號分隔。

			// 使用 JwtService 來獲取 fUserId
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

			couponFromID = 1;
			Factory f = new Factory(_context, _jwtService);
			var coupons = f.getSellerCoupons(couponFromID, searchDTO);
            var result = await coupons.ToListAsync();
            return Ok(coupons);
		}

		// PUT: api/TCoupons/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTCoupon(int id, TCoupon tCoupon)
		{
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
		public async Task<ActionResult<TCoupon>> PostTCoupon(TCoupon tCoupon)
		{
			_context.TCoupons.Add(tCoupon);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTCoupon", new { id = tCoupon.FCouponCodeId }, tCoupon);
		}

		// DELETE: api/TCoupons/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTCoupon(int id)
		{
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

