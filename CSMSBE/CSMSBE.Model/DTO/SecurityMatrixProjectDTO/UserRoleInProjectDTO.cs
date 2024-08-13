using CSMS.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class UserRoleInProjectDTO
    {
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Image {  get; set; }
        public bool? IsPending { get; set; }
        public List<RoleUserProjectDTO> Roles { get; set; }
    }
}
