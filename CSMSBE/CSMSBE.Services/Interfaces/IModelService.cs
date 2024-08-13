using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.Issue;
using CSMS.Model.Model;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface IModelService
    {
        Task<IPagedList<ModelDTO>> FilterModel(ModelFilterRequest filter);
        ModelDTO GetModelById(string id);
        Task<ModelDTO> CreateModel(CreateModelDTO dto, string userId);
        Task<ModelDTO> UpdateModel(UpdateModelDTO dto, string userId);
        Task<ModelDTO> MoveModel(MoveModelDTO dto, string userId);
        Task<bool> RemoveModel(string id);
        Task<List<ModelDTO>> GetTreeModelByParentId(string[] parentIds, bool includeUploaded);
        Task<IPagedList<ModelDTO>> GetDirectChildrenByParentId(string parentId, ModelFilterRequest filter);
        Task<IPagedList<ModelDTO>> FilterModelByProjectId(string projectId, ModelFilterRequest filter);
        Task<List<ModelDTO>> GetTreeModelByProjectId(string projectId, bool includeUploaded);
        Task<SpeckleUploadResult> UploadFileIFC(string modelId, string filePath, string userId);
        Task<ModelVersionDTO> GetModelWithVersionsAsync(string modelId); 
        Task<SpeckleModelInfoDTO> GetSpeckleModelsInfo(SpeckleModelInfoRequest speckleModelInfoRequest);
    }
}
