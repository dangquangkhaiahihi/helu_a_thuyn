using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core.Enum
{
    public enum RecordPermissionEnum
    {
        [Description("SeeOnlyMineRecord")]
        SeeOnlyMineRecord = 1,
        [Description("SeeAllRecord")]
        SeeAllRecord = 2
    }
}
