using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model
{
    public class UserApiLogRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ApiName { get; set; }
        public string IpAddress { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
    }
}
