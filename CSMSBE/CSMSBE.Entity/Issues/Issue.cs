using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.Issues
{
    [Table(TableFieldNameHelper.CSMS.Issue, Schema = Constant.Schema.CSMS)]
    public class Issue : BaseFullAuditedEntity<int>
    {       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string ModelId { get; set; }
        public string? Assignee { get; set; }
        public string? Image { get; set; }


        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
        public virtual Model Model { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
