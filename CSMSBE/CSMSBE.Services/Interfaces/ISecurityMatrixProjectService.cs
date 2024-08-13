using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.RoleProject;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.SecurityMatrixProjectDTO;
using CSMS.Model.Model;
using CSMS.Model.Role;
using CSMS.Model.User;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface ISecurityMatrixProjectService
    {
        Task<bool> HasPermissionByCommentIdAsync(string userId, int commentId, string action);
        Task<bool> HasPermissionByIssueIdAsync(string userId, int issueId, string action);
        Task<bool> HasPermissionByProjectIdAsync(string userId, string projectId, string action);
        Task<bool> HasPermissionByModelIdAsync(string userId, string modelId, string action);
        Task<bool> HasPermissionByDucumentIdAsync(string userId, int documentId, string action);
        Task<IList<ActionProjectDTO>> GetLookupAction(IKeywordDto keywordDto);
        Task<IList<ProjectDTO>> GetLookupProject(IKeywordDto keywordDto, string userId);
        Task<ProjectUserRoleDTO> AssignUser(AssignUserDTO dto, string userId);
        Task<RoleWithActionsDTO> CreateOrUpdateRoleWithActions(CreateUpdateRoleProjectDTO dto, string userId);
        Task<bool> UpdateUserRole(UpdateUserRoleDTO dto);
        Task<bool> RemoveUserProject(string userId, string projectId);
        Task<bool> RemoveRole(string roleId);
        Task<IList<RoleProjectDTO>> GetLookupRoleByProject(IKeywordDto keywordDto, string projectId);
        Task<IPagedList<RoleProjectDTO>> FilterRoleByProjectId(string projectId, RoleProjectFilterRequest filter);
        Task<List<UserRoleInProjectDTO>> GetUsersWithRolesByProjectIdAsync(string projectId);
        Task<bool> UpdateProjectApproval(string projectId, string userId, bool isApproved);
    }
}
