using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]  // 確保所有操作需要通過 JWT 認證
    public class TMemberChatsController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly JwtService _jwtService;

        public TMemberChatsController(dbMSTI161_B_ProjectContext context, IHubContext<ChatHub> hubContext, JwtService jwtService)
        {
            _context = context;
            _hubContext = hubContext;
            _jwtService = jwtService;
        }

        // GET: api/TMemberChats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TMemberChat>>> GetTMemberChats()
        {
            //return await _context.TMemberChats.ToListAsync();

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
            var message = await f.getchatusersAsync();
            return Ok(message);

        }

        // GET: api/TMemberChats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TMemberChat>> GetTMemberChat(int id, int id2)
        {
            //var tMemberChat = await _context.TMemberChats.FindAsync(id);

            //if (tMemberChat == null)
            //{
            //    return NotFound();
            //}

            //return tMemberChat;
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
            var message = await f.getchatmessageAsync(fUserId, id2);
            return Ok(message);
        }

        // PUT: api/TMemberChats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTMemberChat(int id, TMemberChat tMemberChat)
        {
            if (id != tMemberChat.FChatId)
            {
                return BadRequest();
            }

            _context.Entry(tMemberChat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TMemberChatExists(id))
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

        // POST: api/TMemberChats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TMemberChat>> PostTMemberChat(SendMessageDTO request)
        {
            ////_context.TMemberChats.Add(tMemberChat);
            ////await _context.SaveChangesAsync();

            ////return CreatedAtAction("GetTMemberChat", new { id = tMemberChat.FChatId }, tMemberChat);
            //TMemberChat chat = new()
            //{
            //    //UserId = request.UserId,
            //    //ToUserId = request.ToUserId,
            //    //Message = request.Message,
            //    //Date = DateTime.Now
            //    FSenderId = request.senderId,
            //    FReceiverId = request.receiverId,
            //    FMessage = request.message,
            //    FChatImageId = request.FChatImageId,
            //    FSendTime = DateTime.Now
            //};

            //await _context.AddAsync(chat);
            //await _context.SaveChangesAsync();

            //string connectionId = ChatHub.Users.First(p => p.Value == chat.FReceiverId).Key;

            //await _hubContext.Clients.Client(connectionId).SendAsync("Message", chat);

            //return Ok(chat);


            //TMemberChat chat = new TMemberChat
            //{
            //    FSenderId = request.senderId,
            //    FReceiverId = request.receiverId,
            //    FMessage = request.message,
            //    FSendTime = request.sendTime
            //};

            //await _context.TMemberChats.AddAsync(chat);
            //await _context.SaveChangesAsync();

            //// 使用 SignalR Hub 傳送訊息
            //var connectionId = ChatHub.Users.FirstOrDefault(u => u.Value == request.receiverId).Key;
            //if (!string.IsNullOrEmpty(connectionId))
            //{
            //    await _hubContext.Clients.Client(connectionId).SendAsync("Message", new
            //    {
            //        senderName = (await _context.TUsers.FindAsync(request.senderId))?.FName,
            //        receiverName = (await _context.TUsers.FindAsync(request.receiverId))?.FName,
            //        message = request.message,
            //        sendTime = chat.FSendTime
            //    });
            //}
            //else
            //{
            //    // 如果接收者不在線上，則可以記錄該訊息，稍後再發送
            //    return NotFound(new { Message = "Receiver is not connected" });
            //}

            //return Ok(chat);


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

            // 固定使用者為 Eva (fUserId = 1) 發送訊息給接收者 (fReceiverId = 2)
            TMemberChat chat = new TMemberChat
            {
                FSenderId = fUserId,  // Eva 的 ID
                FReceiverId = request.receiverId,  // 測試對象的 ID
                FMessage = request.message,
                FSendTime = DateTime.Now
            };

            await _context.TMemberChats.AddAsync(chat);
            await _context.SaveChangesAsync();

            return Ok(chat);


        }

        // DELETE: api/TMemberChats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTMemberChat(int id)
        {
            var tMemberChat = await _context.TMemberChats.FindAsync(id);
            if (tMemberChat == null)
            {
                return NotFound();
            }

            _context.TMemberChats.Remove(tMemberChat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TMemberChatExists(int id)
        {
            return _context.TMemberChats.Any(e => e.FChatId == id);
        }
    }
}
