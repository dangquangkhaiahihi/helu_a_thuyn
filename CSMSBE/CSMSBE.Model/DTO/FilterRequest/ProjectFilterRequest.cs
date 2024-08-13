using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.FilterRequest
{
    public class ProjectFilterRequest : PagedAndSortResultRequestDto
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? CommuneId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public int? TypeProjectId { get; set; }
        public string? RoleCode { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public void ValidateInput()
        {

        }
    }
}
