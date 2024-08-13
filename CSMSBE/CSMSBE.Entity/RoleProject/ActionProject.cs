using CSMS.Entity.SecurityMatrix;
using CSMSBE.Core.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.RoleProject
{
    [Table(TableFieldNameHelper.SecurityMatrix_Project.ActionProjects, Schema = Constant.Schema.SecurityMatrixProject)]
    public class ActionProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public virtual ICollection<SecurityMatrixProject> SecurityMatrixProjects { set; get; } = new List<SecurityMatrixProject>();
    }
}
