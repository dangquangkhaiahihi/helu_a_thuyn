using CSMSBE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.IssueDTO
{
    public class CommentDTO : BaseModel
    {
        public int? Id { get; set; }
        public string? Content { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public bool? IsDelete { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
