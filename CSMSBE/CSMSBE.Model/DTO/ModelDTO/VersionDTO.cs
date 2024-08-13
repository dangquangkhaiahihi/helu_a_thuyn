using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class VersionDTO
    {
        public int Id { get; set; }
        public string ObjectId { get; set; }
        public string CommitId { get; set; }
        public string BranchName { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string CommitMessage { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public int UploadOrder { get; set; } // Trường tạm thời để định nghĩa thứ tự upload
    }
    public class ModelVersionDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public string SpeckleBranchId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
        public bool IsUpload { get; set; }
        public string PreviewImg { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public List<VersionDTO> Versions { get; set; } = new List<VersionDTO>();
    }
}
