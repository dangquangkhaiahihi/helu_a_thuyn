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
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetRefreshTokens, Schema = "authentication")]
    public class AspNetRefreshTokens : BaseFullAuditedEntity<int>
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string RemoteIpAddress { get; set; }
        public bool Active => DateTime.Now <= Expires;
    }
}
