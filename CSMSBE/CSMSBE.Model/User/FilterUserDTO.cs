using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class FilterUserDTO : PagedResultRequestDto
    {
        public string SortExpression { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }
        public string FullName { get; set; }
    }
}
