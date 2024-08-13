using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMSBE.Infrastructure.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class PushNotificationRepository : BaseRepository<PushNotification>, IPushNotificationRepository
    {
        private readonly csms_dbContext _context;

        public PushNotificationRepository(csms_dbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(PushNotification pushNotification)
        {
            await InsertAsync(pushNotification);
        }

        public async Task<IEnumerable<PushNotification>> GetListNotificationsByUserIdAsync(string appUserId, bool trackChanges)
        {
            var query = Query(n => n.AppUserId.Equals(appUserId));

            if (!trackChanges)
            {
                return await query.AsNoTracking().ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<PushNotification?> GetNotificationByIdAsync(Guid id, bool trackChanges)
        {
            return await FindFirstOrDefaultAsync(n => n != null
                                                      && n.Id.Equals(id), trackChanges);
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var notification = await FindFirstOrDefaultAsync(n => n != null
                                                                  && n.Id.Equals(id)
                                                                  && n.IsRead.Equals(false), trackChanges: true);

            if (notification != null)
            {
                notification.IsRead = true;
            }
        }

    }
}
