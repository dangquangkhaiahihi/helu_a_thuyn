using CSMSBE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class ParentModelDTO : BaseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? Level { get; set; }
        public string? Description { get; set; }
        public string? SpeckleBranchId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? ParentId { get; set; }
        public bool? IsUpload { get; set; }
        public string? PreviewImg { get; set; }
        public string? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public List<ModelDTO> Children { get; set; } = new List<ModelDTO>();
    }
}
