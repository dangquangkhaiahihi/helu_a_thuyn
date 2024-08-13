using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.RoleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class ProjectUserRoleDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
