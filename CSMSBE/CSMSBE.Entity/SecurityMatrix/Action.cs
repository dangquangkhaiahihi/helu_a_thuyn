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
    [Table(TableFieldNameHelper.Sys.Action, Schema = Constant.Schema.SYS)]
    public class Action
    {
        [Column("id")]
        public int Id { set; get; }
        [Column("code")]
        public string Code { set; get; }
        [Column("name")]
        public string Name { set; get; }
        public virtual ICollection<SecurityMatrices> SecurityMatrices { set; get; } = new List<SecurityMatrices>();
    }
}

