using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface INotificationService
    {
        Task<NotificationType> GetNotifycationByNameAsync(string name);
        Task<int> UpdateStatusAsync(int notificationId);
        Task<int> AddNotificationAsync(IList<Notification> notifications);
        Task<int> DeleteStatusAsync(int notificationId);
        Task<Notification> AddNotificationAsync(Notification notification);
        Task<IList<Notification>> GetTenNotificationsAsync(string role, string userId);
        Task<IList<Notification>> GetNotificationsAsync(string role);
        Task<int> CountNotificationsAsync(string role);
        Task<int> CountNotificationsAsync(string role, string userId);
        Task<IList<Notification>> GetNotificationsAsync(string role, string userId);
        Task<string> GetNotificationsSkipAsync(string role, string userId, int skip);
    }
}
