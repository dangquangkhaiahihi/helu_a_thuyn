using AutoMapper;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.Issues;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Model;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
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
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelService> _logger;
        public ModelService(IModelRepository modelRepository, IProjectService projectService, IMapper mapper, ILogger<ModelService> logger)
        {
            _modelRepository = modelRepository;
            _projectService = projectService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IPagedList<ModelDTO>> FilterModel(ModelFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Include(i => i.Project).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
                var resultItems = await pagedQuery
                        .Select(i => new ModelDTO
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Type = i.Type,
                            Level = i.Level,
                            Description = i.Description,
                            SpeckleBranchId = i.SpeckleBranchId,
                            ProjectID = i.ProjectID,
                            ProjectName = i.Project.Name,
                            CreatedDate = i.CreatedDate,
                            CreatedBy = i.CreatedBy,
                            ModifiedDate = i.ModifiedDate,
                            ModifiedBy = i.ModifiedBy,
                            ParentId = i.ParentId,
                            IsUpload = i.IsUpload,
                            Status = i.Status,
                            PreviewImg = i.PreviewImg
                        })
                        .ToListAsync();

                var pagedList = new PagedList<ModelDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IQueryable<CSMS.Entity.CSMS_Entity.Model>> CreateQuery(ModelFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }
            var query = _modelRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Type), x => x.Type.ToLower().Contains(filter.Type.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.CreatedBy), x => x.CreatedBy.ToLower().Contains(filter.CreatedBy.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.ProjectID), x => x.ProjectID.ToLower().Contains(filter.ProjectID.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.ParentId), x => x.ParentId.ToLower().Contains(filter.ParentId.ToLower()));

            return await Task.FromResult(query);
        }

        private IQueryable<CSMS.Entity.CSMS_Entity.Model> ApplySortingToQuery(IQueryable<CSMS.Entity.CSMS_Entity.Model> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(CSMS.Entity.CSMS_Entity.Model), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<CSMS.Entity.CSMS_Entity.Model>(resultExpression);
            }

            return query;
        }
        
        public ModelDTO GetModelById(string id)
        {
            try
            {
                var result = _mapper.Map<ModelDTO>(_modelRepository.Get(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ModelDTO> CreateModel(CreateModelDTO dto, string userId)
        {
            try
            {

                var result = await _modelRepository.Create(dto, userId);
                
                return _mapper.Map<ModelDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ModelDTO> UpdateModel(UpdateModelDTO dto, string userId)
        {
            try
            {
                var updatedEntity = await _modelRepository.Update(dto, userId);
                var modelDTO = _mapper.Map<ModelDTO>(updatedEntity);
                modelDTO.ProjectName = updatedEntity.Project.Name;
                return modelDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> RemoveModel(string id)
        {
            try
            {
                var result = await _modelRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ModelDTO> MoveModel(MoveModelDTO dto, string userId)
        {
            try
            {
                var moveModelEntity = await _modelRepository.MoveModel(dto, userId);
                var modelDTO = _mapper.Map<ModelDTO>(moveModelEntity);
                modelDTO.ProjectName = moveModelEntity.Project.Name;
                return modelDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<ModelDTO>> GetTreeModelByParentId(string[] parentIds, bool includeUploaded)
        {
            try
            {
                var models = await _modelRepository.GetTreeModelByParentId(parentIds, includeUploaded);

                var parentModels = models.Where(m => parentIds.Contains(m.Id)).ToList();
                var result = parentModels.Select(parent => CreateModelDTO(parent, models)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IPagedList<ModelDTO>> GetDirectChildrenByParentId(string parentId, ModelFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = CreateChildQuery(parentId, filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Include(i => i.Project)
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);

                var resultItems = await pagedQuery.ToListAsync();

                var modelIds = resultItems.Select(m => m.Id).ToList();

                var latestModelVersions = await _modelRepository.GetLatestVersionsByModelIdsAsync(modelIds);

                var resultDtos = resultItems.Select(item => new ModelDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    Level = item.Level,
                    Description = item.Description,
                    SpeckleBranchId = item.SpeckleBranchId,
                    ProjectID = item.ProjectID,
                    ProjectName = item.Project.Name,
                    CreatedDate = item.CreatedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedDate = item.ModifiedDate,
                    ModifiedBy = item.ModifiedBy,
                    ParentId = item.ParentId,
                    IsUpload = item.IsUpload,
                    Status = item.Status,
                    PreviewImg = item.PreviewImg,
                    LatestVersion = latestModelVersions.ContainsKey(item.Id)
                        ? new VersionDTO
                        {
                            Id = latestModelVersions[item.Id].Id,
                            ModelId = latestModelVersions[item.Id].ModelId,
                            ModelName = latestModelVersions[item.Id].Model.Name,
                            BranchName = latestModelVersions[item.Id].BranchName,
                            ObjectId = latestModelVersions[item.Id].ObjectId,
                            CommitId = latestModelVersions[item.Id].CommitId,
                            CreatedDate = latestModelVersions[item.Id].CreatedDate
                        }
                        : null
                }).ToList();

                var pagedList = new PagedList<ModelDTO>(resultDtos, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public IQueryable<CSMS.Entity.CSMS_Entity.Model> CreateChildQuery(string parentId, ModelFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }

            var query = _modelRepository.GetDirectChildrenByParentId(parentId)
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Type), x => x.Type.ToLower().Contains(filter.Type.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.CreatedBy), x => x.CreatedBy.ToLower().Contains(filter.CreatedBy.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.ProjectID), x => x.ProjectID.ToLower().Contains(filter.ProjectID.ToLower()));

            return query;
        }

        public async Task<IPagedList<ModelDTO>> FilterModelByProjectId(string projectId, ModelFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = CreateModelProjectQuery(projectId, filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Include(i => i.Project)
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);

                var resultItems = await pagedQuery.ToListAsync();

                var modelIds = resultItems.Select(m => m.Id).ToList();

                var latestModelVersions = await _modelRepository.GetLatestVersionsByModelIdsAsync(modelIds);

                var resultDtos = resultItems.Select(item => new ModelDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    Level = item.Level,
                    Description = item.Description,
                    SpeckleBranchId = item.SpeckleBranchId,
                    ProjectID = item.ProjectID,
                    ProjectName = item.Project.Name,
                    CreatedDate = item.CreatedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedDate = item.ModifiedDate,
                    ModifiedBy = item.ModifiedBy,
                    ParentId = item.ParentId,
                    IsUpload = item.IsUpload,
                    Status = item.Status,
                    PreviewImg = item.PreviewImg,
                    LatestVersion = latestModelVersions.ContainsKey(item.Id)
                        ? new VersionDTO
                        {
                            Id = latestModelVersions[item.Id].Id,
                            ModelId = latestModelVersions[item.Id].ModelId,
                            ModelName = latestModelVersions[item.Id].Model.Name,
                            BranchName = latestModelVersions[item.Id].BranchName,
                            ObjectId = latestModelVersions[item.Id].ObjectId,
                            CommitId = latestModelVersions[item.Id].CommitId,
                            CreatedDate = latestModelVersions[item.Id].CreatedDate
                        }
                        : null
                }).ToList();

                var pagedList = new PagedList<ModelDTO>(resultDtos, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public IQueryable<CSMS.Entity.CSMS_Entity.Model> CreateModelProjectQuery(string projectId, ModelFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }

            var query = _modelRepository.GetModelByProjectId(projectId)
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Type), x => x.Type.ToLower().Contains(filter.Type.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.CreatedBy), x => x.CreatedBy.ToLower().Contains(filter.CreatedBy.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.ProjectID), x => x.ProjectID.ToLower().Contains(filter.ProjectID.ToLower()));

            return query;
        }
        public async Task<List<ModelDTO>> GetTreeModelByProjectId(string projectId, bool includeUploaded)
        {
            try
            {
                var models = await _modelRepository.GetTreeModelByProjectId(projectId, includeUploaded);

                var parentModels = models.Where(m => m.Level == 1).ToList();

                var result = parentModels.Select(parent => CreateModelDTO(parent, models)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        private ModelDTO CreateModelDTO(CSMS.Entity.CSMS_Entity.Model model, List<CSMS.Entity.CSMS_Entity.Model> allModels)
        {
            var children = allModels.Where(child => child.ParentId == model.Id)
                                     .Select(child => CreateModelDTO(child, allModels))
                                     .ToList();

            return new ModelDTO
            {
                Id = model.Id,
                Name = model.Name,
                Level = model.Level,
                Description = model.Description,
                SpeckleBranchId = model.SpeckleBranchId,
                Status = model.Status,
                Type = model.Type,
                ParentId = model.ParentId,
                IsUpload = model.IsUpload,
                PreviewImg = model.PreviewImg,
                ProjectID = model.ProjectID,
                ProjectName = model.Project.Name,
                CreatedBy = model.CreatedBy,
                ModifiedBy = model.ModifiedBy,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate
            };
        }

        public async Task<SpeckleUploadResult> UploadFileIFC(string modelId, string filePath, string userId)
        {
            try
            {
                var uploadEntity = await _modelRepository.UploadFileIFC(modelId, filePath, userId);
                return uploadEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ModelVersionDTO> GetModelWithVersionsAsync(string modelId)
        {
            try
            {
                var result = _mapper.Map<ModelVersionDTO>(await _modelRepository.GetModelWithVersionsAsync(modelId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<SpeckleModelInfoDTO> GetSpeckleModelsInfo(SpeckleModelInfoRequest speckleModelInfoRequest)
        {
            try
            {
                ProjectDTO projectDTO = await _projectService.GetProjectById(speckleModelInfoRequest.projectId);
                string[] modelInfoRequests = speckleModelInfoRequest.requestModelsInfo.Split(",");
                List<string> modelIds = new List<string>();
                List<string> versionIds = new List<string>();
                Dictionary<string, string> modelVersionMapping = new Dictionary<string, string>();

                foreach (string item in modelInfoRequests)
                {
                    var parts = item.Split("@");
                    var modelId = parts[0];
                    modelIds.Add(modelId);

                    if (parts.Length > 1)
                    {
                        var versionId = parts[1];
                        versionIds.Add(versionId);
                        modelVersionMapping[versionId] = modelId;
                    }
                }
                List<SpeckleModelInfo> speckleModelInfos = await _modelRepository.GetSpeckleModelInfo(modelIds);

                foreach (var modelInfo in speckleModelInfos)
                {
                    if (modelInfo.SpeckleModelId != null && modelInfo.SpeckleVersionInfos != null && modelInfo.SpeckleVersionInfos.Length > 1)
                    {
                        foreach (SpeckleVersionInfo version in modelInfo.SpeckleVersionInfos)
                        {
                            if (modelVersionMapping.TryGetValue(version.Id.ToString(), out var correspondingModelId) && correspondingModelId == modelInfo.Id)
                            {
                                modelInfo.SpeckleModelId = $"{modelInfo.SpeckleModelId}@{version.CommitId}";
                                break;
                            }
                        }
                    }
                }

                return new SpeckleModelInfoDTO
                {
                    SpeckleProjectId = projectDTO.SpeckleProjectId,
                    SpeckleModelInfos = speckleModelInfos
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
