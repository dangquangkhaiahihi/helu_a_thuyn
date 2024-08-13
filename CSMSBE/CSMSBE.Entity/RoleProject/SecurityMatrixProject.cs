using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.RoleProject
{
    [Table(TableFieldNameHelper.SecurityMatrix_Project.SecurityMatrixProjects, Schema = Constant.Schema.SecurityMatrixProject)]
    public class SecurityMatrixProject 
    {
        public int Id { set; get; }
        public string RoleId { get; set; }
        public virtual RoleProject Role { get; set; }
        public string ActionId { get; set; }
        public virtual ActionProject Action { get; set; }
    }
}
