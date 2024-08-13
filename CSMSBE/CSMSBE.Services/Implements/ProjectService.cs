using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Entity.CSMS_Entity;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMS.Data.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMSBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using CSMSBE.Core.Interfaces;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Data.Implements;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.Document;
using Microsoft.AspNetCore.SignalR;
using static CSMSBE.Core.Helper.RoleHelper;
using CSMS.Model.DTO.ModelDTO;

namespace CSMSBE.Services.Implements
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ITypeProjectRepository _typeProjectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectService> _logger;
        private readonly ISecurityMatrixProjectRepository _securityMatrixProjectRepository;
        public ProjectService(IProjectRepository projectRepository, 
            ITypeProjectRepository typeProjectRepository, IMapper mapper, 
            ILogger<ProjectService> logger, 
            IDocumentRepository documentRepository,
            ISecurityMatrixProjectRepository securityMatrixProjectRepository)
        {
            _projectRepository = projectRepository;
            _typeProjectRepository = typeProjectRepository;
            _mapper = mapper;
            _logger = logger;
            _documentRepository = documentRepository;
            _securityMatrixProjectRepository = securityMatrixProjectRepository;
        }

        public async Task<ProjectDTO> GetProjectById(string id)
        {
            var projectEntity = await _projectRepository.Get(id);
            if (projectEntity == null)
            {
                throw new ArgumentException("Dự án không tồn tại.");
            }

            var projectDto = _mapper.Map<ProjectDTO>(projectEntity);

            var roleIds = await _securityMatrixProjectRepository.GetAll()
                .Where(ur => ur.ProjectId.Contains(id))
                .Select(ur => ur.RoleId)
                .Distinct()
                .ToListAsync();

            if (roleIds.Any())
            {
                projectDto.RoleProjectId = roleIds.FirstOrDefault();

                var roleCode = _securityMatrixProjectRepository.GetAllRole()
                    .Where(r => roleIds.Contains(r.Id))
                    .Select(r => r.Code)
                    .FirstOrDefault();
                projectDto.RoleProjectCode = roleCode;
            }
            projectDto.TypeProjectName = projectEntity.TypeProject.Name;
            return projectDto;
        }

        public async Task<IPagedList<ProjectDTO>> FilterProject(ProjectFilterRequest filter, string userId)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter, userId);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
                var resultItems = await pagedQuery
                .Select(p => new ProjectDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    SpeckleProjectId = p.SpeckleProjectId,
                    TypeProjectId = p.TypeProjectId,
                    TypeProjectName = p.TypeProject.Name,
                    ProvinceId = p.ProvinceId,
                    DistrictId = p.DistrictId,
                    CommuneId = p.CommuneId,
                    IsPublic = p.IsPublic,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedDate = p.ModifiedDate,
                    RoleProjectId = _securityMatrixProjectRepository.GetAll()
                        .Where(ur => ur.ProjectId == p.Id && ur.UserId == userId)
                        .Select(ur => ur.RoleId)
                        .FirstOrDefault(),
                    RoleProjectCode = _securityMatrixProjectRepository.GetAll()
                        .Where(ur => ur.ProjectId == p.Id && ur.UserId == userId)
                        .Select(ur => _securityMatrixProjectRepository.GetAllRole().Where(r => r.Id == ur.RoleId).Select(r => r.Code).FirstOrDefault())
                        .FirstOrDefault()
                    })
                .ToListAsync();

                var pagedList = new PagedList<ProjectDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IQueryable<Project>> CreateQuery(ProjectFilterRequest filter, string userId)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }
            var projectQuery = _projectRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Code), x => x.Code.ToLower().Contains(filter.Code.ToLower()))
                .WhereIf(filter.ProvinceId.HasValue, x => x.ProvinceId == filter.ProvinceId.Value)
                .WhereIf(filter.DistrictId.HasValue, x => x.DistrictId == filter.DistrictId.Value)
                .WhereIf(filter.CommuneId.HasValue, x => x.CommuneId == filter.CommuneId.Value)
                .WhereIf(filter.TypeProjectId.HasValue, x => x.TypeProjectId == filter.TypeProjectId.Value)
                .WhereIf(!string.IsNullOrEmpty(filter.CreatedBy), x => x.CreatedBy.ToLower().Contains(filter.CreatedBy.ToLower()))
                .Where(x => x.IsDelete == filter.IsDelete);

            
            if (!string.IsNullOrEmpty(filter.RoleCode) && filter.RoleCode.Contains(RoleProjects.ProjectOwnerCode))
            {
                var roleIds = await _securityMatrixProjectRepository.GetRoleIdByCodeAsync(filter.RoleCode);
                if (roleIds == null)
                {
                    throw new ArgumentException("Vai trò không hợp lệ.");
                }
                var allProjectUserRoles = await _securityMatrixProjectRepository.GetAll()
                    .Where(ur => roleIds.Contains(ur.RoleId) && ur.UserId == userId)
                    .Select(ur => ur.ProjectId)
                    .ToListAsync();

                projectQuery = projectQuery
                    .Where(p => allProjectUserRoles.Contains(p.Id));
            }
            else if (!string.IsNullOrEmpty(filter.RoleCode) && !filter.RoleCode.Contains(RoleProjects.ProjectOwnerCode))
            {
                var roleIds = await _securityMatrixProjectRepository.GetRoleIdByCodeAsync(RoleProjects.ProjectOwnerCode);
                if (roleIds == null)
                {
                    throw new ArgumentException("Vai trò không hợp lệ.");
                }
                var allProjectUserRoles = await _securityMatrixProjectRepository.GetAll()
                    .Where(ur => !roleIds.Contains(ur.RoleId) && ur.UserId == userId)
                    .Select(ur => ur.ProjectId)
                    .ToListAsync();

                projectQuery = projectQuery
                    .Where(p => allProjectUserRoles.Contains(p.Id));
            }
            else
            {
                var allProjectUserRoles = await _securityMatrixProjectRepository.GetAll()
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.ProjectId)
                    .ToListAsync();

                projectQuery = projectQuery
                    .Where(p => allProjectUserRoles.Contains(p.Id));
            }
            return await Task.FromResult(projectQuery);
        }

        private IQueryable<Project> ApplySortingToQuery(IQueryable<Project> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(Project), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<Project>(resultExpression);
            }

            return query;
        }


        public async Task<bool> RemoveProject(string id)
        {
            try
            {
                var result = await _projectRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<ProjectDTO > CreateProject(CreateProjectDTO dto, string userId)
        {
            try
            {

                var result = await _projectRepository.Create(dto, userId);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (result != null)
                {
                    /*FileHelper.EnsureFolderExists($@"Tài liệu dự án {dto.Name}");*/
                    var documentCreateDto = new DocumentCreateDto()
                    {
                        Name = $@"Tài liệu dự án {dto.Name}",
                        ProjectId = result.Id,
                        IsFile = false,
                    };
                    var entity = _mapper.Map<Document>(documentCreateDto);
                    var documentCreateResult = await _documentRepository.CreateDocument(entity, userId);
                }
                return _mapper.Map<ProjectDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<ProjectDTO> UpdateProject(UpdateProjectDTO dto, string userId)
        {
            try
            {
                var result = await _projectRepository.Update(dto, userId);
                return _mapper.Map<ProjectDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IList<LookupDTO>> GetLookUpProjectType(IKeywordDto keywordDto)
        {
            try
            {
                var provinces = await _typeProjectRepository.GetLookupTypeProject(keywordDto).ToListAsync();
                if (provinces != null)
                {
                    var lookupProvincesDto = await Task.Run(() => provinces.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupProvincesDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
