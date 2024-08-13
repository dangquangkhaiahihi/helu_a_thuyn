using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public interface IAuditedEntityDto
    {
        string CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        string ModifiedBy { get; set; }

        DateTime ModifiedDate { get; set; }
    }
}
