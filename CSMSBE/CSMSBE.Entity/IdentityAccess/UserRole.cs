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
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetUserRoles, Schema = "authentication")]
    public class UserRole : IdentityUserRole<string>
    {
        public virtual Role Role { get; set; }

        public virtual User User { get; set; }

    }
}
