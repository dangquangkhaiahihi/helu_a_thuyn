using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core
{
    public class GeoCoding
    {
        public List<AddressComponent> address_components { get; set; }
        public List<string> types { get; set; }
        public Geometry geometry { get; set; }
        public string formatted_address { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Geometry
    {
        public Coordinate location { get; set; }
    }

    public class Coordinate
    {
        public string lng { get; set; }
        public string lat { get; set; }
    }
}
