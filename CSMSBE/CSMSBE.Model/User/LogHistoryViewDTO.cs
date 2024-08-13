using CSMSBE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class LogHistoryViewDTO : BaseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Action { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
    }
}
