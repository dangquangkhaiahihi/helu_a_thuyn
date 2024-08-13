using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core.Settings
{
    public class ClientSettings
    {
        public MapDefault MapDefault { get; set; }
    }

    public class MapDefault
    {
        public long Id { get; set; }
    }
}
