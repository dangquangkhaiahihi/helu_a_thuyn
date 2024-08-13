using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model
{
    public class Node
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int? Checked { get; set; } = 0;
        public bool? IsOpen { get; set; } = false;
        public ICollection<Node>? Children { get; set; }
    }
}
