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

    [Table(TableFieldNameHelper.Cms.District, Schema = Constant.Schema.SYS)]
    public class District : BaseFullAuditedEntity<int>
    {
        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(Constant.Maxlength.UnitName)]
        [Column("Code")]
        public string Code { get; set; }

        // Foreign key
        [Column("ProvinceId")]
        public int ProvinceId { get; set; }
        public virtual Province Province { set; get; }

        // Navigation property
        public virtual ICollection<Commune> Communes { get; set; }
        public string GetProvinceName()
        {
            return Province.Name;
        }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
