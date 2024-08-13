using CSMSBE.Core.Helper;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.IdentityAccess
{
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetUserTokens, Schema = "authentication")]

    public class UserTokens : IdentityUserToken<string>
    {
        public DateTimeOffset? ExpiredTime { get; set; }
    }
}
