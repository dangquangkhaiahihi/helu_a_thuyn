using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class UploadAvatarDTO
    {
        public string UserId { get; set; }
        public IFormFile? Avatar { get; set;}
        
    }
}
