using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.Model
{
    public class MoveModelDTO
    {
        public string Id { get; set; }
        public string? NewParentId { get; set; }

        public void ValidateInput()
        {
            
        }
    }
}
