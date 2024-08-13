using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.CSMS_Entity
{
    [Table(TableFieldNameHelper.CSMS.FileExtensions, Schema = Constant.Schema.CSMS)]
    public class FileExtensions : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
