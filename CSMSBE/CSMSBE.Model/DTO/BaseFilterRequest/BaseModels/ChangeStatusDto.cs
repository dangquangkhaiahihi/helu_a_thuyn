using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public class ChangeStatusDto
    {
        public ICollection<long> Ids { get; set; }
        public bool Status { get; set; }
    }
}
