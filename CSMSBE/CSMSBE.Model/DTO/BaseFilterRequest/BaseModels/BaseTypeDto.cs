using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public class BaseTypeDto : FullAuditedEntityDto<long>
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
