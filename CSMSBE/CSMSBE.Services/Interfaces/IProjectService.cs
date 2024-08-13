using CSMSBE.Infrastructure.Interfaces;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.SomeTableDTO;
using CSMSBE.Core.Interfaces;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.ModelDTO;

namespace CSMSBE.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDTO> CreateProject(CreateProjectDTO dto, string userId);
        Task<ProjectDTO> GetProjectById(string id);
        Task<IPagedList<ProjectDTO>> FilterProject(ProjectFilterRequest filter, string userId);
        Task<ProjectDTO> UpdateProject(UpdateProjectDTO dto, string userId);
        Task<bool> RemoveProject(string id);
        Task<IList<LookupDTO>> GetLookUpProjectType(IKeywordDto keywordDto);
    }
}
