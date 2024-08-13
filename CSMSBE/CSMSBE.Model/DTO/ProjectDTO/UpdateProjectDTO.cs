using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ProjectDTO
{
    public class UpdateProjectDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TypeProjectId { get; set; }
        public bool IsPublic { get; set; }
        public void ValidateInput() {}
    }
}
