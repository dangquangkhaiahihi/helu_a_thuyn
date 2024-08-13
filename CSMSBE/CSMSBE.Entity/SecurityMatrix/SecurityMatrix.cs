using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.SecurityMatrix
{
    [Table(TableFieldNameHelper.Sys.SecurityMatrix, Schema = Constant.Schema.SYS)]
    public class SecurityMatrices
    {
        [Column("id")]
        public int Id { set; get; }
        [Column("action_id")]
        public int ActionId { set; get; }
        [Column("screen_id")]
        public int ScreenId { set; get; }
        [Column("role_id")]
        public string RoleId { set; get; }
        public virtual Action Action { set; get; }
        public virtual Screen Screen { set; get; }
        public virtual Role Role { set; get; }
    }
}
