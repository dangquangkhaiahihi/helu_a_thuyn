using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Issue
{
    public class UpdateIssueDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
        public void ValidateInput() 
        {
            if (string.IsNullOrEmpty(Type))
            {
                throw new InvalidOperationException("Type không được để trống");
            }
            if (string.IsNullOrEmpty(Description))
            {
                throw new InvalidOperationException("Description không được để trống");
            }
        }
    }
}
