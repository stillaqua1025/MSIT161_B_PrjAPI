using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ShoppingCartsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;

        public ShoppingCartsController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TRtwshoppingCart>>> GetTRtwshoppingCarts()
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
            //return await _context.TRtwshoppingCarts.ToListAsync();
            Factory f = new Factory(_context, _jwtService);
            var cart = f.getUserShoppingCart(fUserId);
            return Ok(cart);
        }

        // GET: api/ShoppingCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TRtwshoppingCart>> GetTRtwshoppingCart(int id)
        {
            var tRtwshoppingCart = await _context.TRtwshoppingCarts.FindAsync(id);

            if (tRtwshoppingCart == null)
            {
                return NotFound();
            }

            return tRtwshoppingCart;
        }

        // PUT: api/ShoppingCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutTRtwshoppingCart([FromBody]ShoppingCartDTO shoppingCartDto)
        {

            // 從數據庫查詢該購物車記錄
            var shoppingCart = await _context.TRtwshoppingCarts.FindAsync(shoppingCartDto.scid);

            // 如果購物車不存在
            if (shoppingCart == null)
            {
                return NotFound("找不到該購物車");
            }

            // 更新購物車的數量
            shoppingCart.FQty = shoppingCartDto.quantity;

            // 標記實體為已修改狀態
            _context.Entry(shoppingCart).State = EntityState.Modified;

            try
            {
                // 保存更改
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRtwshoppingCartExists(shoppingCartDto.scid))
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

        // POST: api/ShoppingCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TRtwshoppingCart>> PostTRtwshoppingCart([FromBody]AddCartDTO cartdto)
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

            //組資料
            Factory f = new Factory(_context, _jwtService);
            TRtwshoppingCart tRtwshoppingCart = f.InsertShoppingCart(cartdto, fUserId);
            _context.TRtwshoppingCarts.Add(tRtwshoppingCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTRtwshoppingCart", new { id = tRtwshoppingCart.FShoppingCartId }, tRtwshoppingCart);
        }

        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTRtwshoppingCart(int id)
        {
            var tRtwshoppingCart = await _context.TRtwshoppingCarts.FindAsync(id);
            if (tRtwshoppingCart == null)
            {
                return NotFound();
            }

            _context.TRtwshoppingCarts.Remove(tRtwshoppingCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TRtwshoppingCartExists(int id)
        {
            return _context.TRtwshoppingCarts.Any(e => e.FShoppingCartId == id);
        }
    }
}
