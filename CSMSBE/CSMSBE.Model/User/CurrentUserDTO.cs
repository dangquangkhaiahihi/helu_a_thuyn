using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class CurrentUserDTO
    {
        public string Id { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }
        public string? AvatarUrl { get; set; }
        public List<RoleDTO> Roles { set; get; }
        public string UserType { get; set; }
    }

    public class RoleDTO
    {
        public string? Id { set; get; }
        public string? Code { set; get; }      
        public string? Name { set; get; }
        public string? CreatedBy { set; get; }
        public DateTimeOffset? CreatedDate { set; get; }
        public string? ModifiedBy { set; get; }
        public DateTimeOffset? ModifiedDate { set; get; }
    }
    public class UserRoleDTO
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
