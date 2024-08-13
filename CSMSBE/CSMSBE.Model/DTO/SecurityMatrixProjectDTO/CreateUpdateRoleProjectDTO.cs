using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class CreateUpdateRoleProjectDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProjectId { get; set; }
        public List<string> ActionIds { get; set; }
    }
}
