using CSMS.Data.Repository;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.Issues;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.Issue;
using CSMS.Model.Model;
using CSMSBE.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CSMS.Data.Interfaces
{
    public interface IModelRepository : IBaseRepository<Entity.CSMS_Entity.Model>
    {
        Entity.CSMS_Entity.Model Get(string id);
        Task<Entity.CSMS_Entity.Model> Create(CreateModelDTO dto, string userId);
        Task<Entity.CSMS_Entity.Model> Update(UpdateModelDTO dto, string userId);
        Task<bool> Remove(string id);
        Task<Entity.CSMS_Entity.Model> MoveModel(MoveModelDTO dto, string userId);
        Task<List<Entity.CSMS_Entity.Model>> GetTreeModelByParentId(string[] parentIds, bool includeUploaded);
        IQueryable<Entity.CSMS_Entity.Model> GetDirectChildrenByParentId(string parentId);
        IQueryable<Entity.CSMS_Entity.Model> GetModelByProjectId(string projectId);
        Task<List<Entity.CSMS_Entity.Model>> GetTreeModelByProjectId(string projectId, bool includeUploaded);
        Task<SpeckleUploadResult> UploadFileIFC(string modelId, string filePath, string userId);
        Task<ModelVersionDTO> GetModelWithVersionsAsync(string modelId);
        Task<Dictionary<string, ModelVersion>> GetLatestVersionsByModelIdsAsync(List<string> modelIds);
        Task<List<SpeckleModelInfo>> GetSpeckleModelInfo(List<string> modelIds);
    }
}
