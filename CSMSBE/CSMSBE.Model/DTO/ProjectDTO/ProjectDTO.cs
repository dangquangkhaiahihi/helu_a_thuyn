using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Services;


namespace CSMS.Model.DTO.ProjectDTO
{
    public class ProjectDTO : BaseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? SpeckleProjectId { get; set; }
        public int? TypeProjectId { get; set; }
        public string? TypeProjectName { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? CommuneId { get; set; }
        public bool? IsPublic { get; set; }
        public string? RoleProjectId { get; set; }
        public string? RoleProjectCode { get; set;}
    }
}
