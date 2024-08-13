using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core.SendMailServices
{
    public class EmailConfiguration
    {
        public string EmailHost { get; set; }
        public string EmailFrom { get; set; }
        public int EmailPost { get; set; }
        public string EmailUser { get; set; }
        public string EmailPass { get; set; }
        public string EmailAdmin { get; set; }
    }
}
