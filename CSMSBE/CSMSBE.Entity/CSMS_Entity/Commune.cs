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
    [Table(TableFieldNameHelper.Cms.Commune, Schema = Constant.Schema.SYS)]
    public class Commune : BaseFullAuditedEntity<int>
    {
        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Code")]
        public string Code { get; set; }

        // Foreign key
        [Column("DistrictId")]
        public int DistrictId { get; set; }
        public virtual District District { set; get; }

        public string GetDistrictName()
        {
            return District.Name;
        }

        // Navigation property
        public virtual ICollection<Project> Projects { get; set; }
    }
}
