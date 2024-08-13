using AutoMapper;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.RoleProject;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.SecurityMatrixProjectDTO;
using CSMS.Model.User;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class SecurityMatrixProjectService : ISecurityMatrixProjectService
    {
        private readonly ISecurityMatrixProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SecurityMatrixProjectService> _logger;
        public SecurityMatrixProjectService(ISecurityMatrixProjectRepository repository, IMapper mapper, ILogger<SecurityMatrixProjectService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> HasPermissionByCommentIdAsync(string userId, int commentId, string action)
        {
            var project = await _repository.GetProjectByCommentIdAsync(commentId);

            var userRole = await _repository.GetUserRoleInProjectAsync(userId, project.Id);
            if (userRole == null)
            {
                return false;
            }

            var securityMatrix = await _repository.GetSecurityMatrixByRoleIdAndActionAsync(userRole.RoleId, action);
            return securityMatrix != null;
        }
        public async Task<bool> HasPermissionByProjectIdAsync(string userId, string projectId, string action)
        {
            var userRole = await _repository.GetUserRoleInProjectAsync(userId, projectId);
            if (userRole == null)
            {
                return false;
            }

            var securityMatrix = await _repository.GetSecurityMatrixByRoleIdAndActionAsync(userRole.RoleId, action);
            return securityMatrix != null;
        }
        public async Task<bool> HasPermissionByModelIdAsync(string userId, string modelId, string action)
        {
            var project = await _repository.GetProjectByModelId(modelId);
            var userRole = await _repository.GetUserRoleInProjectAsync(userId, project.Id);
            if (userRole == null)
            {
                return false;
            }

            var securityMatrix = await _repository.GetSecurityMatrixByRoleIdAndActionAsync(userRole.RoleId, action);
            return securityMatrix != null;
        }
        public async Task<bool> HasPermissionByDucumentIdAsync(string userId, int documentId, string action)
        {
            var project = await _repository.GetProjectByDocumentId(documentId);
            var userRole = await _repository.GetUserRoleInProjectAsync(userId, project.Id);
            if (userRole == null)
            {
                return false;
            }

            var securityMatrix = await _repository.GetSecurityMatrixByRoleIdAndActionAsync(userRole.RoleId, action);
            return securityMatrix != null;
        }
        public async Task<bool> HasPermissionByIssueIdAsync(string userId, int issueId, string action)
        {
            var project = await _repository.GetProjectByIssueIdAsync(issueId);

            var userRole = await _repository.GetUserRoleInProjectAsync(userId, project.Id);
            if (userRole == null)
            {
                return false;
            }

            var securityMatrix = await _repository.GetSecurityMatrixByRoleIdAndActionAsync(userRole.RoleId, action);
            return securityMatrix != null;
        }
        
        public async Task<IList<ActionProjectDTO>> GetLookupAction(IKeywordDto keywordDto)
        {
            try
            {
                var actions = await _repository.GetLookupAction(keywordDto).ToListAsync();
                if (actions != null)
                {
                    var actionsDto = _mapper.Map<List<ActionProjectDTO>>(actions);
                    return actionsDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IList<ProjectDTO>> GetLookupProject(IKeywordDto keywordDto, string userId)
        {
            try
            {
                var projects = await _repository.GetLookupProjectAsync(keywordDto, userId);
                if (projects != null)
                {
                    var projectsDto = _mapper.Map<List<ProjectDTO>>(projects);
                    return projectsDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ProjectUserRoleDTO> AssignUser(AssignUserDTO dto, string userId)
        {
            try
            {
                var projectUserRole = await _repository.AssignUser(dto, userId);

                var projectUserRoleDTO = new ProjectUserRoleDTO
                {
                    UserId = projectUserRole.UserId,
                    UserName = projectUserRole.Users?.UserName,
                    ProjectId = projectUserRole.ProjectId,
                    ProjectName = projectUserRole.Projects?.Name,
                    RoleId = projectUserRole.RoleId,
                    RoleName = projectUserRole.Roles?.Name
                };

                return projectUserRoleDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<RoleWithActionsDTO> CreateOrUpdateRoleWithActions(CreateUpdateRoleProjectDTO dto, string userId)
        {
            var result = await _repository.CreateOrUpdateRoleWithActions(dto, userId);
            return result;
        }

        public async Task<bool> UpdateUserRole(UpdateUserRoleDTO dto)
        {
            var result = await _repository.UpdateUserRole(dto.UserId, dto.ProjectId, dto.RoleId);
            return result;
        }

        public async Task<bool> RemoveUserProject(string userId, string projectId)
        {
            var result = await _repository.RemoveUserProject(userId, projectId);
            return result;
        }


        public async Task<bool> RemoveRole(string roleId)
        {
            var result = await _repository.RemoveRole(roleId);
            return result;
        }

        public async Task<IList<RoleProjectDTO>> GetLookupRoleByProject(IKeywordDto keywordDto, string projectId)
        {
            try
            {
                var roles = await _repository.GetLookupRoleByProject(keywordDto, projectId).ToListAsync();
                if (roles != null)
                {
                    var rolesDto = _mapper.Map<List<RoleProjectDTO>>(roles);
                    return rolesDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        private IQueryable<RoleProject> ApplySortingToQuery(IQueryable<RoleProject> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(RoleProject), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<RoleProject>(resultExpression);
            }

            return query;
        }
        public async Task<IPagedList<RoleProjectDTO>> FilterRoleByProjectId(string projectId, RoleProjectFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = CreateQuery(projectId, filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);

                var resultItems = await pagedQuery.ToListAsync();

                var roleIds = resultItems.Select(item => item.Id).ToList();
                var roleActions = await _repository.GetAllAction()
                    .Where(smp => roleIds.Contains(smp.RoleId))
                    .GroupBy(smp => smp.RoleId)
                    .Select(g => new
                    {
                        RoleId = g.Key,
                        ActionIds = g.Select(smp => smp.ActionId).ToList()
                    })
                    .ToListAsync();

                var resultDtos = resultItems.Select(item => new RoleProjectDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    IsDefault = item.IsDefault,
                    ProjectId = item.ProjectId,
                    ProjectName = item.Projects.Name,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ActionIds = roleActions.FirstOrDefault(ra => ra.RoleId.Equals(item.Id))?.ActionIds ?? new List<string>()
                }).ToList();

                var pagedList = new PagedList<RoleProjectDTO>(resultDtos, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public IQueryable<RoleProject> CreateQuery(string projectId, RoleProjectFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }
            var query = _repository.GetAllRole()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Code), x => x.Code.ToLower().Contains(filter.Code.ToLower()))
                .Where(r => r.IsDelete == false && r.ProjectId == projectId);
            return query;
        }
        public async Task<List<UserRoleInProjectDTO>> GetUsersWithRolesByProjectIdAsync(string projectId)
        {
            var result = await _repository.GetUsersWithRolesByProjectIdAsync(projectId);
            return result;
        }

        public async Task<bool> UpdateProjectApproval(string projectId, string userId, bool isApproved)
        {
            var result = await _repository.UpdateProjectApprovalAsync(projectId, userId, isApproved);
            return result;
        }
    }
}
