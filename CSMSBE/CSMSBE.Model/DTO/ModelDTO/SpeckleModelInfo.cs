using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class SpeckleModelInfoDTO
    {
        public string? SpeckleProjectId { get; set; }
        public List<SpeckleModelInfo>? SpeckleModelInfos { get; set; }
    }

    public class SpeckleModelInfo
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? SpeckleModelId { get; set; }
        public SpeckleVersionInfo[]? SpeckleVersionInfos { get; set; }
    }

    public class SpeckleVersionInfo
    {
        public int Id { get; set; }
        public string? CommitId { get; set; }
        public string? ObjectId { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
