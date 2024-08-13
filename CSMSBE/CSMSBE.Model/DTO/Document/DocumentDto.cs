using CSMS.Entity.CSMS_Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.Document
{
    public class DocumentDto : Node
    {
        public ICollection<DocumentDto>? Children { get; set; } = new List<DocumentDto>();
        public int Id { get; set; }
        // File or Folder
        public bool IsFile { get; set; }
        public string? UrlPath { get; set; }
        public long? Size { get; set; }
        public string Icon { get; set; }
        public string ProjectId { get; set; }
        public string? FileExtension { get; set; }
        public int ParentId { get; set; }
        public string Status { get; set; }
    }
}
