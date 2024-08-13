using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.Issues;
using CSMS.Entity.RoleProject;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.SecurityMatrixProjectDTO;
using CSMS.Model.Model;
using CSMS.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface ISecurityMatrixProjectRepository
    {
        Task<ProjectUserRole> GetUserRoleInProjectAsync(string userId, string projectId);
        Task<SecurityMatrixProject> GetSecurityMatrixByRoleIdAndActionAsync(string roleId, string action);
        Task<Project> GetProjectByModelId(string modelId);
        Task<Project> GetProjectByDocumentId(int documentId);
        Task<Project> GetProjectByIssueIdAsync(int issueId);
        Task<Project> GetProjectByCommentIdAsync(int commentId);
        IQueryable<ActionProject> GetLookupAction(IKeywordDto keywordDto);
        Task<IQueryable<Project>> GetLookupProjectAsync(IKeywordDto keywordDto, string userId);
        Task<ProjectUserRole> AssignUser(AssignUserDTO dto, string userId);
        Task<RoleWithActionsDTO> CreateOrUpdateRoleWithActions(CreateUpdateRoleProjectDTO dto, string userId);
        Task<bool> UpdateUserRole(string userId, string projectId, string roleId);
        Task<bool> RemoveUserProject(string userId, string projectId);
        Task<bool> RemoveRole(string roleId);
        IQueryable<ProjectUserRole> GetAll();
        IQueryable<RoleProject> GetAllRole();
        IQueryable<SecurityMatrixProject> GetAllAction();
        Task<List<string>> GetRoleIdByCodeAsync(string roleCode);
        IQueryable<RoleProject> GetLookupRoleByProject(IKeywordDto keywordDto, string projectId);
        Task<List<UserRoleInProjectDTO>> GetUsersWithRolesByProjectIdAsync(string projectId);
        Task<bool> UpdateProjectApprovalAsync(string projectId, string userId, bool isApproved);
        
        }
}
