using CSMS.Entity.IdentityAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    public class PushNotification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public string AppUserId { get; set; }
        public virtual User AppUser { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
    }
}
