using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Core.Helper;

using static System.Net.Mime.MediaTypeNames;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.SomeTable, Schema = Constant.Schema.CSMS)]

    public class SomeTable : BaseFullAuditedEntity<long>
    {
        [Column("normal_text")]
        public string NormalText { get; set; }
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("type")]
        public string Type { get; set; }
        
    }
}
