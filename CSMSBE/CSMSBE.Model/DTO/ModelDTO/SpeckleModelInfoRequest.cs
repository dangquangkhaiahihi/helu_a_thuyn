using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class SpeckleModelInfoRequest
    {
        public string? projectId { get; set; }
        public string? requestModelsInfo { get; set; }

        public void ValidateInput()
        {
            if (projectId == null || requestModelsInfo == null) {
                throw new Exception("projectId and requestModelsInfo are required.");
            }
        }
    }
}
