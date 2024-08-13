using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity;

namespace CSMS.Model.DTO.SomeTableDTO
{
    public class SomeTableDTO : BaseFullAuditedEntity<int>
    {
        public int? Id { get; set; }
        public string? NormalText { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public string? Type { get; set; }
    }
}
