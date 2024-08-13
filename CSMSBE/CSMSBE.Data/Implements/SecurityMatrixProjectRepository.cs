using AutoMapper;
using Castle.Core.Logging;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.Issues;
using CSMS.Entity.RoleProject;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.BaseFilterRequest.BaseModels;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMS.Model.DTO.SecurityMatrixProjectDTO;
using CSMS.Model.User;
using CSMSBE.Core.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSMSBE.Core.Helper.RoleHelper;

namespace CSMS.Data.Implements
{
    public class SecurityMatrixProjectRepository : ISecurityMatrixProjectRepository
    {
        private readonly csms_dbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SecurityMatrixProjectRepository> _logger;
        public SecurityMatrixProjectRepository(csms_dbContext context, IMapper mapper, ILogger<SecurityMatrixProjectRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RoleProject> GetRoleProjectAsync(string projectId)
        {
            return await _context.RoleProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }

        public async Task<ProjectUserRole> GetUserRoleInProjectAsync(string userId, string projectId)
        {
            return await _context.ProjectUserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProjectId == projectId);
        }

        public async Task<SecurityMatrixProject> GetSecurityMatrixByRoleIdAndActionAsync(string roleId, string action)
        {
            return await _context.SecurityMatrixProjects
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.Action.Code == action);
        }

        public async Task<Project> GetProjectByModelId(string modelId)
        {
            var project = await _context.Models
                    .Where(m => m.Id == modelId)
                    .Select(m => m.Project)
                    .FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentException("Project không tồn tại");
            }
            return project;
        }
        public async Task<Project> GetProjectByDocumentId(int documentId)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(m => m.Id == documentId);
            if (document == null) throw new ArgumentException("Document không tồn tại");
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == document.ProjectId);
            if (project == null) throw new ArgumentException("Project không tồn tại");
            return project;
        }
        public IQueryable<ActionProject> GetLookupAction(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<ActionProject> query = null;
                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.ActionProjects;
                    return query;
                }
                query = _context.ActionProjects.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IQueryable<Project>> GetLookupProjectAsync(IKeywordDto keywordDto, string userId)
        {
            try
            {
                var user = await GetUsersById(userId);
                if (user == null) throw new ArgumentException("User không tồn tại");
                IQueryable<Project> query = null;
                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Projects.Where(p => p.CreatedBy == user.UserName);
                    return query;
                }
                query = _context.Projects.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()) 
                                                && x.CreatedBy == user.UserName);
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProjectUserRole> AssignUser(AssignUserDTO dto, string userId)
        {
            try
            {
                var currentUser = await GetUsersById(userId);
                if (currentUser == null) throw new ArgumentException("Người dùng hiện tại không tồn tại.");

                var userToAssign = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
                if (userToAssign == null) throw new ArgumentException("Người dùng được mời không tồn tại.");

                var project = await GetProjectById(dto.ProjectId);
                var viewerRole = await _context.RoleProjects
                    .FirstOrDefaultAsync(p => p.Code.Equals(RoleProjects.ViewerCode) && p.ProjectId == dto.ProjectId);

                if (project == null) throw new ArgumentException("Dự án không tồn tại.");
                if (viewerRole == null) throw new ArgumentException("Vai trò mặc định không tồn tại.");

                var existingRole = await _context.ProjectUserRoles
                    .FirstOrDefaultAsync(pur => pur.UserId == userToAssign.Id && pur.ProjectId == dto.ProjectId);

                if (existingRole != null)
                    throw new ArgumentException($"Người dùng {userToAssign.UserName} đã ở trong dự án {project.Name}.");

                var assignUserEntity = new ProjectUserRole
                {
                    UserId = userToAssign.Id,
                    ProjectId = dto.ProjectId,
                    RoleId = viewerRole.Id,
                    Users = userToAssign,
                    Projects = project,
                    Roles = viewerRole,
                    IsPending = true
                };

                _context.ProjectUserRoles.Add(assignUserEntity);
                await _context.SaveChangesAsync();

                return assignUserEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AssignUser");
                throw;
            }
        }

        public async Task<RoleWithActionsDTO> CreateOrUpdateRoleWithActions(CreateUpdateRoleProjectDTO dto, string userId)
        {
            try
            {
                var user = await GetUsersById(userId);
                if (user == null) throw new ArgumentException("User không tồn tại");
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == dto.ProjectId);
                if (project == null) throw new ArgumentException("Dự án không tồn tại");

                var role = await _context.RoleProjects.FindAsync(dto.Id);

                if (role == null)
                {
                    role = new RoleProject
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = dto.Name,
                        Code = dto.Code,
                        ProjectId = project.Id,
                        IsDefault = false
                    };
                    role.SetDefaultValue(user.UserName);
                    role.SetValueUpdate(user.UserName);
                    _context.RoleProjects.Add(role);
                }
                else
                {
                    role.Name = dto.Name;
                    role.SetValueUpdate(user.UserName);
                    _context.RoleProjects.Update(role);
                }

                await _context.SaveChangesAsync();

                var actions = await _context.ActionProjects
                    .Where(a => dto.ActionIds.Contains(a.Id))
                    .ToListAsync();

                var existingActions = await _context.SecurityMatrixProjects
                    .Where(smp => smp.RoleId == role.Id)
                    .ToListAsync();

                _context.SecurityMatrixProjects.RemoveRange(existingActions);

                var securityMatrixProjects = actions.Select(a => new SecurityMatrixProject
                {
                    RoleId = role.Id,
                    ActionId = a.Id
                }).ToList();

                _context.SecurityMatrixProjects.AddRange(securityMatrixProjects);
                await _context.SaveChangesAsync();

                var result = new RoleWithActionsDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    Actions = actions.Select(a => new ActionProjectDTO
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList()
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating or updating role with actions");
                throw;
            }
        }

        public async Task<bool> UpdateUserRole(string userId, string projectId, string roleId)
        {
            try
            {
                var user = await GetUsersById(userId);
                if (user == null) throw new ArgumentException("User không tồn tại");

                var role = _context.RoleProjects.FirstOrDefault(u => u.Id == roleId);
                if (role == null) throw new ArgumentException("Vị trí không tồn tại");
                if (role.Code.Equals(RoleProjects.ProjectOwnerCode))
                    throw new ArgumentException($"Không thể thiết lập vị trí {role.Name} cho người dùng khác!");
                var project = await GetProjectById(projectId);

                var currentRole = _context.ProjectUserRoles.Include(pur => pur.Roles)
                                                                .FirstOrDefault(p => p.UserId == user.Id
                                                                && p.ProjectId == projectId);

                if (currentRole.Roles.Code.Equals(RoleProjects.ProjectOwnerCode)) 
                    throw new ArgumentException($"Người dùng {user.UserName} là Chủ sở hữu dự án, không thể cập nhật thành vị trí khác.");
                if (currentRole.IsPending == true) 
                    throw new ArgumentException($"Người dùng {user.UserName} chưa tham gia vào dự án.");
                if (currentRole == null) 
                    throw new ArgumentException($"Người dùng {user.UserName} không thuộc dự án dự án.");
                currentRole.RoleId = role.Id;
                _context.ProjectUserRoles.Update(currentRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveUserProject(string userId, string projectId)
        {
            var userRoleProject = await CurrentUserRole(userId, projectId);
            if (userRoleProject == null) 
                throw new ArgumentException($"Người dùng không thuộc dự án này.");
            if (userRoleProject.Roles.Code.Equals(RoleProjects.ProjectOwnerCode)) 
                throw new ArgumentException($"Không thể xóa {userRoleProject.Roles.Name} ra khỏi dự án.");
            _context.ProjectUserRoles.Remove(userRoleProject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRole(string roleId)
        {
            var role = _context.RoleProjects.FirstOrDefault(r => r.Id.Equals(roleId));
            var viewerRole = _context.RoleProjects.FirstOrDefault(rp => rp.Code.Equals(RoleProjects.ViewerCode) 
                                                                && rp.ProjectId.Equals(role.ProjectId));
            if (role == null) return false;
            if (role.IsDefault == true) throw new ArgumentException("Không thể xóa vị trí mặc định!");

            var existUser = _context.ProjectUserRoles.Where(pur => pur.RoleId == roleId).ToList();
            if (existUser.Any())
            {
                foreach (var user in existUser)
                {
                    user.RoleId = viewerRole.Id;
                    _context.ProjectUserRoles.Update(user);
                }
            }
            _context.RoleProjects.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Project> GetProjectById(string projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id.Equals(projectId));
        }
        private async Task<User> GetUsersById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }
        private async Task<ProjectUserRole> CurrentUserRole(string userId, string projectId)
        {
            return await _context.ProjectUserRoles.Include(pur => pur.Roles).FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.ProjectId.Equals(projectId));
        }

        public IQueryable<ProjectUserRole> GetAll()
        {
            return _context.ProjectUserRoles.AsQueryable();
        }

        public async Task<List<string>> GetRoleIdByCodeAsync(string roleCode)
        {
            return await _context.RoleProjects
                .Where(r => roleCode == r.Code)
                .Select(r => r.Id)
                .ToListAsync();
        }

        public IQueryable<RoleProject> GetAllRole()
        {
            return _context.RoleProjects.AsQueryable();
        }

        public IQueryable<RoleProject> GetLookupRoleByProject(IKeywordDto keywordDto, string projectId)
        {
            try
            {
                IQueryable<RoleProject> query = null;

                var project = _context.Projects.Find(projectId);
                if (project == null || string.IsNullOrEmpty(projectId))
                    throw new ArgumentException("Project không tồn tại");

                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.RoleProjects
                        .Where(rp => projectId.Contains(rp.ProjectId)
                                     && rp.IsDelete == false);
                }
                else
                {
                    query = _context.RoleProjects
                        .Where(rp => projectId.Contains(rp.ProjectId)
                                     && rp.Name.ToLower().Contains(keywordDto.Keyword.ToLower())
                                     && rp.IsDelete == false);
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Project> GetProjectByIssueIdAsync(int issueId)
        {
            var issue = await _context.Issues.FirstOrDefaultAsync(m => m.Id == issueId);
            if (issue == null) throw new ArgumentException("Issue không tồn tại");
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == issue.ProjectId);
            if (project == null) throw new ArgumentException("Project không tồn tại");
            return project;
        }

        public async Task<Project> GetProjectByCommentIdAsync(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(m => m.Id == commentId);
            if (comment == null) throw new ArgumentException("Comment không tồn tại");
            var issue = await _context.Issues.FirstOrDefaultAsync(m => m.Id == comment.IssueId);
            if (issue == null) throw new ArgumentException("Issue không tồn tại");
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == issue.ProjectId);
            if (project == null) throw new ArgumentException("Project không tồn tại");
            return project;
        }
        public async Task<List<UserRoleInProjectDTO>> GetUsersWithRolesByProjectIdAsync(string projectId)
        {
            return await _context.ProjectUserRoles
                .Where(pur => pur.ProjectId == projectId)
                .Select(pur => new UserRoleInProjectDTO
                {
                    UserId = pur.UserId,
                    FullName = _context.Users.Where(u => u.Id == pur.UserId).Select(u => u.FullName).FirstOrDefault(),
                    Image = _context.Users.Where(u => u.Id == pur.UserId).Select(u => u.AvatarUrl).FirstOrDefault(),
                    UserName = _context.Users.Where(u => u.Id == pur.UserId).Select(u => u.UserName).FirstOrDefault(),
                    Email = _context.Users.Where(u => u.Id == pur.UserId).Select(u => u.Email).FirstOrDefault(),
                    Phone = _context.Users.Where(u => u.Id == pur.UserId).Select(u => u.PhoneNumber).FirstOrDefault(),
                    IsPending = pur.IsPending,
                    Roles = _context.RoleProjects
                        .Where(rp => _context.ProjectUserRoles
                            .Any(pur2 => pur2.UserId == pur.UserId && pur2.RoleId == rp.Id))
                        .Select(rp => new RoleUserProjectDTO
                        {
                            Id = rp.Id,
                            Name = rp.Name,
                            Code = rp.Code
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<bool> UpdateProjectApprovalAsync(string projectId, string userId, bool isApproved)
        {
            var user = GetUsersById(userId);
            if (user == null) throw new ArgumentException($"Người dùng không tồn tại");

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) throw new ArgumentException($"Dự án không tồn tại");

            var userProject = await CurrentUserRole(userId, project.Id);

            if (userProject == null)
                throw new ArgumentException($"Người dùng chưa được mời vào dự án {project.Name}");

            if (!isApproved)
            {
                _context.ProjectUserRoles.Remove(userProject);
            }
            else
            {
                userProject.IsPending = false;
                _context.ProjectUserRoles.Update(userProject);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<SecurityMatrixProject> GetAllAction()
        {
            return _context.SecurityMatrixProjects.AsQueryable();
        }
    }
}
