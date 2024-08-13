using CSMSBE.Core.Helper;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.SecurityMatrix
{
    [Table(TableFieldNameHelper.Sys.Screen, Schema = Constant.Schema.SYS)]
    public class Screen
    {
        [Column("id")]
        public int Id { set; get; }
        [Column("parent_id")]
        public int? ParentId { set; get; }
        [ForeignKey(nameof(ParentId))]
        public virtual Screen Parent { get; set; }

        [Column("code")]
        public string Code { set; get; }
        [Column("name")]
        public string Name { set; get; }
        [Column("title")]
        public string Title { set; get; }
        [Column("icon")]
        public string Icon { set; get; }
        [Column("order")]
        public int Order { get; set; }
        public virtual ICollection<SecurityMatrices> SecurityMatrices { set; get; } = new List<SecurityMatrices>();
        public virtual ICollection<Screen> Childrent { get; set; }
    }
}
