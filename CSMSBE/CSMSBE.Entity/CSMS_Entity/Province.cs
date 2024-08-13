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
    [Table(TableFieldNameHelper.Cms.Province, Schema = Constant.Schema.SYS)]
    public class Province : BaseFullAuditedEntity<int>
    {
        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Code")]
        public string Code { get; set; }

        // Navigation property
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}