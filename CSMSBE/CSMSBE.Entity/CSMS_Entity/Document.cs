using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.Document, Schema = Constant.Schema.CSMS)]
    public class Document : BaseFullAuditedEntity<int>
    {    
        public virtual ICollection<Document>? Children { get; set; } = new List<Document>();
        public int? ParentId { get; set; }
        public virtual Document? Parent { get; set; }
        public string Name { get; set; }
        // File or Folder
        public bool IsFile { get; set; }
        public string? Status { get; set; }
        public string? UrlPath { get; set; }
        public long? Size { get; set; }
        public string? FileExtension { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
