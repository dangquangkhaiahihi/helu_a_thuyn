using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.FilterRequest
{
    public class ModelFilterRequest : PagedAndSortResultRequestDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? ProjectID { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? ParentId {  get; set; }  
        public void ValidateInput()
        {

        }
    }
}
