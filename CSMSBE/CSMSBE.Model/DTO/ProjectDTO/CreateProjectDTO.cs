using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity;
using CSMSBE.Services;

namespace CSMS.Model.DTO.ProjectDTO
{
    public class CreateProjectDTO
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? CommuneId { get; set; }
        public int TypeProjectId { get; set; }
        public bool IsPublic { get; set; }
        public void ValidateInput() { }
    }
}
