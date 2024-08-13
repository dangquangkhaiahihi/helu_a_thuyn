using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Issue
{
    public class CreateCommentDTO
    {
        public string? Content { get; set; }
        public int IssueId { get; set; }
        public int? ParentId { get; set; }
        public void ValidateInput() 
        {
            if (string.IsNullOrEmpty(Content))
            {
                throw new InvalidOperationException("Comment không được để trống");
            }
        }
    }
}
