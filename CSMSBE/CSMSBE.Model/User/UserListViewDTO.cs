using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class UserListViewDTO
    {
        public string Id { set; get; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string Address { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string RoleName { set; get; }
        public string PhoneNumber { set; get; }
        public string UserType { get; set; }
        public string LastDateLogin { get; set; }

    }
}
