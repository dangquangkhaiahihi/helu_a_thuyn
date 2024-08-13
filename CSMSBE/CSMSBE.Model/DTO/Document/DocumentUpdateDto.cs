using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.Document
{
    public class DocumentUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void ValidateInput()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("Name không được để trống");
            }
        }
    }
}
