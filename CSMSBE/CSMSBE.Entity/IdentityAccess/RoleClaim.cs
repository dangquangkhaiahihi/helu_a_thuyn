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
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetRoleClaims, Schema = "authentication")]

    public class RoleClaim : IdentityRoleClaim<string>
    {
        [Key]
        public override string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
