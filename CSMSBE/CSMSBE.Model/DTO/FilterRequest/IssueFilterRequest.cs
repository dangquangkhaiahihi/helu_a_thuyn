using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.FilterRequest
{
    public class IssueFilterRequest : PagedAndSortResultRequestDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }        
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        
        public void ValidateInput()
        {

        }
    }
}
