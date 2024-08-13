using CSMS.Entity.CSMS_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IPushNotificationRepository
    {
        Task AddNotificationAsync(PushNotification pushNotification);
        Task<IEnumerable<PushNotification>> GetListNotificationsByUserIdAsync(string appUserId, bool trackchanges);
        Task<PushNotification?> GetNotificationByIdAsync(Guid id, bool trackchanges);
        Task MarkAsReadAsync(Guid id);
    }
}
