using CSMSBE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Role
{
    public class RoleViewDTO : BaseModel
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
    }
}
