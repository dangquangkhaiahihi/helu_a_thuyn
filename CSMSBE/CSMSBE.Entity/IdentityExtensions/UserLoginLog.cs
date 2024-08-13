
using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.IdentityExtensions
{
    [Table(TableFieldNameHelper.Sys.UserLoginLog, Schema = Constant.Schema.SYS)]
    public class UserLoginLog : BaseFullAuditedEntity<int>
    {
        [Column("user_id")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Column("api_name")]
        public string ApiName { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }
        
        [Column("full_name")]
        public string FullName { get; set; }
    }
}
