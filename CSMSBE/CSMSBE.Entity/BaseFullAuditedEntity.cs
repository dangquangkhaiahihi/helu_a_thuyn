using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity
{
    public class BaseFullAuditedEntity<T> : BaseEntity<T>
    {
        public virtual void SetDefaultValue(string createBy)
        {
            CreatedDate = DateTimeOffset.Now;
            CreatedBy = createBy;
            IsDelete = false;
        }
        public virtual void SetValueUpdate(string updateBy)
        {
            ModifiedDate = DateTimeOffset.Now;
            ModifiedBy = updateBy;
            IsDelete = false;
        }

        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("created_date")]
        public DateTimeOffset CreatedDate { get; set; }

        [Column("modified_by")]
        public string? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTimeOffset ModifiedDate { get; set; }
        [Column("is_delete")]
        public bool? IsDelete { get; set; }
    }
}
