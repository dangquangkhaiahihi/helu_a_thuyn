using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.Document
{
    public class DocumentCreateDto
    {
        public IFormFile? File { get; set; }
        public string? Name { get; set; }
        public string ProjectId { get; set; }
        public bool IsFile { get; set; }
        public int? ParentId { get; set; }
        public void ValidateInput()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("Name không được để trống");
            }
        }
    }
}
