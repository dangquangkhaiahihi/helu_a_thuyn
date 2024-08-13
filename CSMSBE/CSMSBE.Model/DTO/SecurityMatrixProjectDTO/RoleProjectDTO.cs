using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixProjectDTO
{
    public class RoleProjectDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool? IsDefault { get; set; }
        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? CreatedBy { get; set;}
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get;set; }
        public List<string>? ActionIds { get; set; }
    }
    public class RoleUserProjectDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
