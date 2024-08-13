using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSMS.Entity.RoleProject
{
    [Table(TableFieldNameHelper.SecurityMatrix_Project.ProjectUserRoles, Schema = Constant.Schema.SecurityMatrixProject)]
    public class ProjectUserRole
    {
        public int Id { set; get; }

        public string? UserId { get; set; }
        [JsonIgnore]
        public virtual User Users { get; set; }

        public string? ProjectId { get; set; }
        [JsonIgnore]
        public virtual Project Projects { get; set; }

        public string? RoleId { get; set; }
        [JsonIgnore]
        public virtual RoleProject Roles { get; set; }
        public bool? IsPending { get; set; }
    }
}
