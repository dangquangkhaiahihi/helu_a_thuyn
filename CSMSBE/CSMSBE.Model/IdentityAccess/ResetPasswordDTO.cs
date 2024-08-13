using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.IdentityAccess
{
    public class ResetPasswordDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
    public class CheckPasswordDTO
    {
        public string Password { get; set; }
    }
    public class ResetPasswordAdminDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
