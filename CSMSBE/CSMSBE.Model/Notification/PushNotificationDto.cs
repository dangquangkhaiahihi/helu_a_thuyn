using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Notification
{
    public class PushNotificationDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public DateTimeOffset SentDate { get; set; }
        public bool IsRead { get; set; }
    }
}
