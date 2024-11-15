using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Repositories;
using MSIT161_B_PriAPI.Controllers;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.DTOs;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // 確保所有操作需要通過 JWT 認證
    public class OrdersController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;
        public OrdersController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TRtworder>>> GetTRtworders()
        {
            //原本 return await _context.TRtworders.ToListAsync();

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

            // 根據 fUserId 查詢該使用者的訂單
            var orders = await _context.TRtworders.Where(o => o.FUserId == fUserId).ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public IActionResult GetTRtworder(int id)
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

            Factory f = new Factory(_context, _jwtService);
            var orders = f.getUserOders(fUserId); //參數id改成fUserId
            return Ok(orders);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTRtworder(int id, TRtworder tRtworder)
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

            // 確認此訂單是否屬於該使用者
            if (id != tRtworder.FOrderId || tRtworder.FUserId != fUserId)
            {
                return BadRequest();
            }

            _context.Entry(tRtworder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRtworderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TRtworder>> PostTRtworder(CheckOutDTO dto)
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
            //新增訂單
            Factory f = new Factory(_context, _jwtService);
            TRtworder tRtworder = f.InsertOders(dto, fUserId);
            _context.TRtworders.Add(tRtworder);
            await _context.SaveChangesAsync();
            //新增訂單明細
            int orderId = tRtworder.FOrderId;
            List<TRtworderDetail> details = await f.InsertOrderDetail(fUserId, orderId);
            _context.TRtworderDetails.AddRange(details);
            await _context.SaveChangesAsync();
            //清空購物車
            var carts = await f.deleteUserShoppingCarts(fUserId);
            if( carts!=null && carts.Count != 0)
            {
                _context.TRtwshoppingCarts.RemoveRange(carts);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetTRtworder", new { id = tRtworder.FOrderId }, tRtworder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTRtworder(int id)
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


            var tRtworder = await _context.TRtworders.FindAsync(id);
            if (tRtworder == null)
            {
                return NotFound();
            }

            _context.TRtworders.Remove(tRtworder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TRtworderExists(int id)
        {
            return _context.TRtworders.Any(e => e.FOrderId == id);
        }
    }
}
