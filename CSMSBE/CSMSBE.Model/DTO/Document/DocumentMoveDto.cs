using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.Document
{
    public class DocumentMoveDto
    {
        public int DocumentId { get; set; }
        public int DestinationId { get; set; }
        public void ValidateInput()
        {

        }
    }
}
