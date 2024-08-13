using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.Cms.TypeProject, Schema = Constant.Schema.SYS)]
    public class TypeProject : BaseFullAuditedEntity<int>
    {
        [Column("Name")]
        [StringLength(Constant.Maxlength.Name)]
        public string Name { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }

}
