using CSMS.Entity.CSMS_Entity;
using CSMS.Model.Notification;
using CSMSBE.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface IPushNotificationService
    {
        Task<Result<IEnumerable<PushNotificationDto>>> GetListNotificationsByUserId(string userId);
        Task<Result<PushNotificationDto>> GetNotificationById(Guid id);
        Task<Result<string>> MarkAsRead(Guid id);
    }
}
