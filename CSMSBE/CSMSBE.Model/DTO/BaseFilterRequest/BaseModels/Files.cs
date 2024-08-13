using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public class Files
    {
        public string fileName { get; set; }
        public string fileType { get; set; }
        public string filePreview { get; set; }
        public long fileSize { get; set; }
        public long? fileId { get; set; }
    }
}
