using CSMS.Entity.SecurityMatrix;
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
    [Table(TableFieldNameHelper.IdentityExtentions.AspNetRoles, Schema = "authentication")]
    public class Role : IdentityRole<string>
    {
        public Role() { }

        public Role(string roleName)
            : base(roleName)
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
        public virtual ICollection<RoleClaim> RoleClaims { get; } = new List<RoleClaim>();
        public virtual ICollection<SecurityMatrices> SecurityMatrices { set; get; } = new List<SecurityMatrices>();
    }
}
