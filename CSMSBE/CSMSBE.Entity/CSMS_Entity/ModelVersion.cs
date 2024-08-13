using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.ModelVersion, Schema = Constant.Schema.CSMS)]
    public class ModelVersion : BaseFullAuditedEntity<int>
    {
        public string? ObjectId { get; set; }
        public string? CommitId { get; set; }
        public string? BranchName { get; set; }
        public string ModelId { get; set; }

        public virtual Model Model { get; set; }
    }
}
