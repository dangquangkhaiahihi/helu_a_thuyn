using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.SecurityMatrix
{
    public class CreateSecurityMatrixDTO
    {
        public string RoleId { set; get; }
        public List<Screen> Screens { set; get; }
    }

    public class Screen
    {
        public int ScreenId { get; set; }
        public List<Action> Actions { set; get; }
    }

    public class Action
    {
        public int ActionId { get; set; }
    }
}
