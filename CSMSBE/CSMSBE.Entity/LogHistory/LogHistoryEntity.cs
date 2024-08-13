using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.LogHistory
{
    [Table("log_history", Schema = Constant.Schema.CMS)]
    public class LogHistoryEntity : BaseFullAuditedEntity<int>
    {
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("action")]
        public int Action { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
