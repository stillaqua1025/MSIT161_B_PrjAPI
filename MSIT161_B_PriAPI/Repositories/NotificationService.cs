using Microsoft.AspNetCore.SignalR;
using MSIT161_B_PriAPI.Hubs;
using MSIT161_B_PriAPI.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MSIT161_B_PriAPI.Repositories
{
    public class NotificationService
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(dbMSTI161_B_ProjectContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task NotifyUser(int userId, int notifyTypeId)
        {
            var userNotification = await _context.TRtwnotifies.FirstOrDefaultAsync(n => n.FUserId == userId && n.FNotifyTypeId == notifyTypeId && n.FNotifyState == true);

            if (userNotification != null)
            {
                // 根據通知類型設置內容
                var notificationMessage = userNotification.FNotify;

            // 使用 SignalR 推送通知
            //await _hubContext.Clients.User(userId.ToString())
            //    .SendAsync("ReceiveNotification", notificationMessage);

            var message = "優惠券成功領取";
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);

            // 推播完成後，將通知標記為未讀
            userNotification.FIsNotRead = false;  // 標記為未讀
            await _context.SaveChangesAsync();    // 保存變更到資料庫
        }
    }
    }
}
