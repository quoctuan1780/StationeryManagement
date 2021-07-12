using static Common.Constant;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ShopDbContext _context;

        public NotificationService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);

            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<IList<Notification>> GetTenNotificationsAsync(string role, string userId)
        {
            return await _context.Notifications.Include(x => x.User).Include(x => x.NotificationType).Where(x => x.RoleSeen.Equals(role) && x.UserId.Equals(userId)).OrderByDescending(x => x.CreatedDate).Take(10).ToListAsync();
        }

        public async Task<NotificationType> GetNotifycationByNameAsync(string name)
        {
            return await _context.NotificationTypes.Where(x => x.NotificationTypeName.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateStatusAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if(notification != null)
            {
                notification.Status = STATUS_SEEN_NOTIFICATION;

                _context.Notifications.Update(notification);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<Notification>> GetNotificationsAsync(string role)
        {
            return await _context.Notifications.Include(x => x.User).Include(x => x.NotificationType).Where(x => x.RoleSeen.Equals(role)).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<int> DeleteStatusAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification != null)
            {
                notification.IsDeleted = true;

                _context.Notifications.Update(notification);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountNotificationsAsync(string role)
        {
            return await _context.Notifications.Where(x => x.RoleSeen.Equals(role) && x.Status.Equals(STATUS_NOT_SEEN_NOTIFICATION)).CountAsync();
        }

        public async Task<int> CountNotificationsAsync(string role, string userId)
        {
            return await _context.Notifications
                .Include(x => x.NotificationType)
                .Where(x => x.RoleSeen.Equals(role) && x.Status.Equals(STATUS_NOT_SEEN_NOTIFICATION) && x.UserId.Equals(userId)).CountAsync();
        }

        public async Task<IList<Notification>> GetNotificationsAsync(string role, string userId)
        {
            return await _context.Notifications
                .Include(x => x.NotificationType)
                .Where(x => x.RoleSeen.Equals(role) && x.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<int> AddNotificationAsync(IList<Notification> notifications)
        {
            await _context.Notifications.AddRangeAsync(notifications);

            return await _context.SaveChangesAsync();
        }
    }
}
