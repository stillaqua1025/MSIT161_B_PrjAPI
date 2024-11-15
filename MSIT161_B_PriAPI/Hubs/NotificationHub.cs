using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Repositories;


namespace MSIT161_B_PriAPI.Hubs
{
    public class NotificationHub:Hub
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly NotificationService _notificationService;

        public NotificationHub(NotificationService notificationService, dbMSTI161_B_ProjectContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }

        public async Task SendNotificationToUser(int userId, int notifyTypeId)
        {
            await _notificationService.NotifyUser(userId, notifyTypeId);
        }

    }
}
