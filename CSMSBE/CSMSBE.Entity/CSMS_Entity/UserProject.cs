using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.UserProject, Schema = Constant.Schema.CSMS)]
    public class UserProject : BaseFullAuditedEntity<long>
    {
        [Column("ProjectId")]
        public int ProjectId { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("Permissions")]
        public string Permissions { get; set; }
        [Column("RoleId")]
        public int RoleId { get; set; }
    }
}
