using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSMSBE.Core.Helper;

namespace CSMS.Entity.IdentityAccess
{
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetUserClaims, Schema = "authentication")]

    public class UserClaim : IdentityUserClaim<string>
    {
        [Key]
        public override string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
