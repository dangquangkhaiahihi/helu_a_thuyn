using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Core.Helper;
using Microsoft.EntityFrameworkCore;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.RoleProject;
using static CSMSBE.Core.Helper.RoleHelper;
using CSMSBE.Model.Repository;
using CSMS.Entity.LogHistory;
using CSMSBE.Core.Extensions;

namespace CSMS.Data.Implements
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<ProjectRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectRepository(csms_dbContext context, ILogger<ProjectRepository> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        private User GetCurrentUser(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) throw new ArgumentException("User không tồn tại!");
            return user;
        }
        private async Task AddActionsToRoles(List<RoleProject> roles)
        {
            var actions = new List<SecurityMatrixProject>();

            foreach (var role in roles)
            {
                var roleActions = GetActionsForRole(role.Code);

                foreach (var actionId in roleActions)
                {
                    var securityMatrixProject = CreateRoleActions(role.Id, actionId);
                    actions.Add(securityMatrixProject);
                }
            }
            await _context.SecurityMatrixProjects.AddRangeAsync(actions);
            await _context.SaveChangesAsync();
        }
        private SecurityMatrixProject CreateRoleActions(string roleId, string actionId)
        {
            var securityMatrixProject = new SecurityMatrixProject
            {
                RoleId = roleId,
                ActionId = actionId
            };
            return securityMatrixProject;
        }
        private IEnumerable<string> GetActionsForRole(string roleCode)
        {
            switch (roleCode)
            {
                case RoleProjects.ProjectOwnerCode:
                    return new[] { "A1", "A2", "A3", "A4", "A5", "A6" };
                case RoleProjects.ProjectManagerCode:
                    return new[] { "A2", "A3", "A4", "A5", "A6" };
                case RoleProjects.BIMEngineerCode:
                    return new[] { "A3", "A4", "A5" };
                case RoleProjects.ReviewerCode:
                    return new[] { "A4", "A5", "A6" };
                case RoleProjects.ViewerCode:
                    return new[] { "A4", "A5" };
                default:
                    return Enumerable.Empty<string>();
            }
        }
        private RoleProject CreateRole(string userName, string projectId, string name, string code)
        {
            var role = new RoleProject
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Code = code,
                IsDefault = true,
                ProjectId = projectId
            };
            role.SetDefaultValue(userName);
            role.SetValueUpdate(userName);

            return role;
        }
        private async Task<List<RoleProject>> CreateDefaultRole(string userName, string projectId)
        {
            var roles = new List<RoleProject>
                {
                    CreateRole(userName, projectId, RoleProjects.ProjectOwnerName, RoleProjects.ProjectOwnerCode),
                    CreateRole(userName, projectId, RoleProjects.ProjectManagerName, RoleProjects.ProjectManagerCode),
                    CreateRole(userName, projectId, RoleProjects.BIMEngineerName, RoleProjects.BIMEngineerCode),
                    CreateRole(userName, projectId, RoleProjects.ReviewerName, RoleProjects.ReviewerCode),
                    CreateRole(userName, projectId, RoleProjects.ViewerName, RoleProjects.ViewerCode)
                };

            await _context.RoleProjects.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            return roles;
        }
        public async Task<Project> Create(CreateProjectDTO dto, string userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            var projectEntity = _mapper.Map<Project>(dto);
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    try
                    {
                        var user = GetCurrentUser(userId);

                        string speckleProjectId = await SpeckleHelper.CreateStream(dto.Name, dto.Description, true);
                        projectEntity.SpeckleProjectId = speckleProjectId;
                        projectEntity.Id = Guid.NewGuid().ToString();
                        projectEntity.SetDefaultValue(user.UserName);
                        projectEntity.SetValueUpdate(user.UserName);

                        var result = _context.Projects.Add(projectEntity);
                        var roles = await CreateDefaultRole(user.UserName, projectEntity.Id);

                        var projectUserRole = new ProjectUserRole
                        {
                            UserId = userId,
                            ProjectId = projectEntity.Id,
                            RoleId = roles.First(r => r.Name == RoleProjects.ProjectOwnerName).Id,
                            IsPending = false
                        };
                        _context.ProjectUserRoles.Add(projectUserRole);
                        await AddActionsToRoles(roles);

                        await _unitOfWork.CompleteAsync();
                        transaction.Commit();
                        return result.Entity;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during CreateProject");
                        await LogTransaction("CreateProject", projectEntity.SpeckleProjectId.ToString(), null, null, null, null, "Failed");
                        throw ex;
                    }
            });
        }

        public async Task<Project> Get(string id)
        {           
            try
            {
                var result = await _context.Projects
                    .Include(p => p.Models.Where(m => m.IsDelete == false))
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IQueryable<Project> GetAll()
        {
            try
            {
                var result = _context.Projects.AsQueryable();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<bool> Remove(string id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            var entity = _context.Projects.FirstOrDefault(x => x.Id.Equals(id));
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    try
                    {
                        
                        if (entity == null)
                        {
                            return false;
                        }
                        _context.Projects.Remove(entity);
                        var result = await SpeckleHelper.DeleteStream(entity.SpeckleProjectId);
                        await _unitOfWork.CompleteAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during DeleteModel");
                        await LogTransaction("RemoveProject", entity.SpeckleProjectId, null, null, null, null, "Failed");
                        throw ex;
                    }
            });
        }
        
        public async Task<Project> Update(UpdateProjectDTO updateDto, string userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            var entity = _context.Projects.Find(updateDto.Id);
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    try
                    {
                        var user = GetCurrentUser(userId);
                        
                        if (entity == null)
                        {
                            throw new ArgumentException("Không tìm thấy bản ghi để cập nhật");
                        }

                        // Update fields
                        _context.Entry(entity).CurrentValues.SetValues(updateDto);
                        var result = await SpeckleHelper.UpdateStream(entity.SpeckleProjectId, updateDto.Name, updateDto.IsPublic, updateDto.Description);
                        if (!result)
                        {
                            throw new Exception("Cập nhật speckle project thất bại");
                        }
                        entity.SetValueUpdate(user.UserName);
                        await _unitOfWork.CompleteAsync();
                        transaction.Commit();
                        return entity;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during UpdateProject");
                        await LogTransaction("UpdateProject", entity.SpeckleProjectId.ToString(), null, null, null, null, "Failed");
                        throw ex;
                    }
            });
        }
        private async Task LogTransaction(string transactionType, string modelId, string oldParentId, string newParentId, string newBranchName, string speckleBranchId, string status)
        {
            var transactionLog = new TransactionLog
            {
                TransactionType = transactionType,
                ModelId = modelId,
                OldParentId = oldParentId,
                NewParentId = newParentId,
                NewBranchName = newBranchName,
                SpeckleBranchId = speckleBranchId,
                CURRENT_TIMESTAMP = DateTimeOffset.UtcNow,
                status = status
            };
            _context.TransactionLogs.Add(transactionLog);
            await _context.SaveChangesAsync();
        }
    }
}
