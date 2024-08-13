using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.LogHistory
{
    [Table("TransactionLog", Schema = Constant.Schema.CMS)]
    public class TransactionLog : BaseFullAuditedEntity<int>
    {
        public int Id { get; set; }
        public string? TransactionType { get; set; }
        public string? ModelId { get; set; }
        public string? OldParentId { get; set; }
        public string? NewParentId { get; set; }
        public string? NewBranchName { get; set; }
        public string? SpeckleBranchId { get; set;}
        public DateTimeOffset? CURRENT_TIMESTAMP { get; set; }
        public string? status { get; set; }
    }
}
