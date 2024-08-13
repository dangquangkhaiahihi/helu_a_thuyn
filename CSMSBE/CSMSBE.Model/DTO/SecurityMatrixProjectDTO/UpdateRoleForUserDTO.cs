using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class UpdateRoleForUserDTO
    {
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string newRoleId { get; set; }
    }
}
