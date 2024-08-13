using CSMS.Entity.CSMS_Entity;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.RoleProject
{
    [Table(TableFieldNameHelper.SecurityMatrix_Project.RoleProjects, Schema = Constant.Schema.SecurityMatrixProject)]
    public class RoleProject : BaseFullAuditedEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public bool? IsDefault { get; set; }    
        public string? ProjectId {  get; set; }
        public virtual ICollection<SecurityMatrixProject> SecurityMatrixProjects { get; set; }
        public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; }
        public virtual Project Projects { get; set; }
    }
}
