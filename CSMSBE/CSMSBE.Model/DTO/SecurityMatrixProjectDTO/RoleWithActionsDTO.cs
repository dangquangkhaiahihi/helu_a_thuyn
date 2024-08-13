using CSMS.Model.DTO.SecurityMatrixDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class RoleWithActionsDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<ActionProjectDTO> Actions { get; set; }
    }
}
