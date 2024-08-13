using CSMS.Entity.Issues;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table("Model", Schema = Constant.Schema.CSMS)]
    public class Model : BaseFullAuditedEntity<string>
    {
        public string Id { get; set; }
        public string ProjectID { get; set; }
        public virtual Project Project { set; get; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public string? SpeckleBranchId { get; set; }
        public string? SpeckleBranchName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string? ParentId { get; set; }
        public bool IsUpload { get; set; }
        public string? PreviewImg { get; set; }

        [ForeignKey("ParentId")]
        public virtual Model Parent { get; set; }
        public virtual ICollection<Model> Children { get; set; } 
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<ModelVersion> ModelVersion { get; set; }
    }
}
