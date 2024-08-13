using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.SecurityMatrix
{
    public class ActionViewDTO
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }

    public class ScreenViewDTO
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public List<ActionViewDTO> Actions { get; set; }
    }
}
