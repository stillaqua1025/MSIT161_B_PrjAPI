using Microsoft.AspNetCore.SignalR;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;

namespace MSIT161_B_PriAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        public static Dictionary<string, int> Users = new();
        public ChatHub(dbMSTI161_B_ProjectContext context)
        {
            _context = context;
        }
        public async Task Connect(int userId)
        {
            Users.Add(Context.ConnectionId, userId);
            TUser? user = await _context.TUsers.FindAsync(userId);
            if (user is not null)
            {
                await Clients.All.SendAsync("TUsers", user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int userId;
            Users.TryGetValue(Context.ConnectionId, out userId);
            Users.Remove(Context.ConnectionId);
            TUser? user = await _context.TUsers.FindAsync(userId);
            if (user is not null)
            {
                await Clients.All.SendAsync("TUsers", user);
            }
        }


        //private readonly dbMSTI161_B_ProjectContext _context;
        //public static Dictionary<string, int> Users = new();

        //public ChatHub(dbMSTI161_B_ProjectContext context)
        //{
        //    _context = context;
        //}

        //public async Task Connect(int userId)
        //{
        //    Users[Context.ConnectionId] = userId;
        //    var user = await _context.TUsers.FindAsync(userId);
        //    if (user != null)
        //    {
        //        await Clients.All.SendAsync("TUsers", user);  // 通知所有人該用戶上線
        //    }
        //}

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    if (Users.TryGetValue(Context.ConnectionId, out var userId))
        //    {
        //        Users.Remove(Context.ConnectionId);
        //        var user = await _context.TUsers.FindAsync(userId);
        //        if (user != null)
        //        {
        //            await Clients.All.SendAsync("TUsers", user);  // 通知所有人該用戶離線
        //        }
        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}

        public async Task SendMessage(SendMessageDTO message)
        {
            var chat = new TMemberChat
            {
                FSenderId = message.senderId,
                FReceiverId = message.receiverId,
                FMessage = message.message,
                FSendTime = DateTime.Now
            };
            await _context.TMemberChats.AddAsync(chat);
            await _context.SaveChangesAsync();

            //if (Users.ContainsValue(message.receiverId))
            //{
            //var connectionId = Users.FirstOrDefault(u => u.Value == message.receiverId).Key;
            //await Clients.Client(connectionId).SendAsync("Message", chat);
            //}
            // 找到接收者的 connectionId
            var receiverConnectionId = Users.FirstOrDefault(u => u.Value == message.receiverId).Key;

            // 找到發送者的 connectionId
            var senderConnectionId = Users.FirstOrDefault(u => u.Value == message.senderId).Key;

            // 如果找到接收者和發送者的 connectionId
            //if (receiverConnectionId != null && senderConnectionId != null)
            //{
            // 將發送者和接收者的 connectionId 加入到一個列表中
            var connectionIds = new List<string> { senderConnectionId, receiverConnectionId };

            // 使用 Clients.Clients 同時將訊息傳送給發送者和接收者
            await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", chat);
            //}
        }
    }
}

