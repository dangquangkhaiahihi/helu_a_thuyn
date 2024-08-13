using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.SecurityMatrix
{
    public class SecurityMatrixListViewDTO
    {
        public int Id { set; get; }
        public string RoleName { set; get; }
        public string RoleId { set; get; }
        public int ScreenId { set; get; }
        public string ScreenName { set; get; }
        public List<ActionLookupDTO> Actions { set; get; }
    }
}
