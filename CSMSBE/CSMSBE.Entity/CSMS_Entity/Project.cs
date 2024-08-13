using CSMS.Entity.Issues;
using CSMS.Entity.RoleProject;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.Project, Schema = Constant.Schema.CSMS)]
    public class Project : BaseFullAuditedEntity<string>
    {
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }

        [Column("Description")]
        public string Description { get; set; }
        [Column("SpeckleProjectId")]
        public string SpeckleProjectId { get; set; }
        // Foreign keys
        public int TypeProjectId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? CommuneId { get; set; }
        public bool IsPublic { get; set; }
        // Navigation properties
        public virtual TypeProject TypeProject { get; set; }
        public virtual Province Province { get; set; }
        public virtual District District { get; set; }
        public virtual Commune Commune { get; set; }

        public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; } = new List<ProjectUserRole>();
        public virtual ICollection<RoleProject.RoleProject> RoleProjects { get; set; }
    }
}
