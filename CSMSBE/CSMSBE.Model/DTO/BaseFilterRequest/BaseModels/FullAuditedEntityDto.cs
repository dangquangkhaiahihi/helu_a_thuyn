using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public class FullAuditedEntityDto<T> : EntityDto<T>, IFullAuditedEntityDto<T>
    {
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("modified_by")]
        public string ModifiedBy { get; set; }
        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
