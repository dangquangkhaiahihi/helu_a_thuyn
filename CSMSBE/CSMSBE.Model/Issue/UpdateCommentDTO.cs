using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Issue
{
    public class UpdateCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public void ValidateInput() 
        {
            if (string.IsNullOrEmpty(Content))
            {
                throw new InvalidOperationException("Comment không được để trống");
            }
        }
    }
}
