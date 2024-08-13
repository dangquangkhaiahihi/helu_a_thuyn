using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.Issues;
using CSMS.Entity.LogHistory;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.Model;
using CSMSBE.Core.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using Speckle.Core.Api.SubscriptionModels;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CSMSBE.Model.Repository;
namespace CSMS.Data.Implements
{
    public class ModelRepository : BaseRepository<Entity.CSMS_Entity.Model>, IModelRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<ModelRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ModelRepository(csms_dbContext context, ILogger<ModelRepository> logger, IMapper mapper, IUnitOfWork unitOfWork) : base(context)
        {
            _context = context; 
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Entity.CSMS_Entity.Model> Create(CreateModelDTO dto, string userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                try
                {

                    dto.ValidateInput();

                    var user = GetUserById(userId);
                    var project = GetProjectById(dto.ProjectID);
                    var modelEntity = _mapper.Map<Entity.CSMS_Entity.Model>(dto);
                    modelEntity.Id = Guid.NewGuid().ToString();
                    var parent = GetModelById(dto.ParentId);
                    string speckleBranchId = null;

                    if (parent != null && parent.IsUpload == true)
                    {
                        throw new ArgumentException("Model đã upload file, không thể tạo mới thêm");
                    }

                    //Trường hợp không có parent
                    if (parent == null)
                    {
                        await CreateBranch(dto.Name, project.SpeckleProjectId, dto.Description)
                            .ContinueWith(task =>
                            {
                                if (task.Exception != null) throw task.Exception;
                                speckleBranchId = task.Result;
                            });
                            modelEntity.Level = 1;
                        modelEntity.ParentId = null;
                    }
                    //Trường hợp có parent
                    else
                    {
                        var remainingChildren = _context.Models.Where(m => m.ParentId == parent.Id).ToList();
                            if (remainingChildren.Any())
                            {
                                await CreateBranch(dto.Name, project.SpeckleProjectId, dto.Description)
                                    .ContinueWith(task =>
                                    {
                                        if (task.Exception != null) throw task.Exception;
                                        speckleBranchId = task.Result;
                                    });
                            }
                            else
                            {
                                speckleBranchId = parent.SpeckleBranchId;
                                await UpdateBranch(speckleBranchId, dto.Name, project.SpeckleProjectId, dto.Description);
                            }        
                        modelEntity.Level = parent.Level + 1;
                        parent.Type = "FOLDER";
                        parent.SpeckleBranchId = null;
                        modelEntity.ParentId = parent.Id;
                        _context.Models.Update(parent);
                    }

                    modelEntity.SpeckleBranchId = speckleBranchId;
                    modelEntity.Status = "NEW";
                    modelEntity.Type = "MODEL";
                    modelEntity.SetDefaultValue(user.UserName);
                    modelEntity.SetValueUpdate(user.UserName);

                    _context.Models.Add(modelEntity);

                    await _unitOfWork.CompleteAsync();
                    transaction.Commit();
                    return modelEntity;
                }
                catch (Exception ex)
                {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during CreateModel");
                        await LogTransaction("CreateBranch", null, null, null, null, null, "Failed");
                        throw;
                }
            });
        }

        public Entity.CSMS_Entity.Model Get(string id)
        {           
            try
            {

                var result = _context.Models
                  .Include(i => i.Project)
                  .ThenInclude(m => m.Models)
                  .FirstOrDefault(x => x.Id == id);
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
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                try
                {
                    var entity = GetModelById(id);
                    var children = GetAllChildren(entity.Id);
                    var project = GetProjectById(entity.ProjectID);
                    if (entity == null || project == null)
                    {
                        return false;
                    }
                        var removeEntity = GetModelById(id);
                        _context.Models.Remove(removeEntity);
                        if (entity.SpeckleBranchId != null)
                        {
                            await DeleteBranch(entity.SpeckleBranchId, project.SpeckleProjectId);
                        }
                    var childBranch = GetAllChildren(entity.Id);
                    if (children.Any())
                    {
                        _context.Models.RemoveRange(children);
                        foreach (var child in childBranch)
                        {
                                try
                                {
                                    if (child.SpeckleBranchId != null)
                                    {
                                        await DeleteBranch(child.SpeckleBranchId, project.SpeckleProjectId);
                                    }
                                }
                                catch (Exception removeChildEx)
                                {
                                    _logger.LogError(removeChildEx, "Error during updating model");
                                    await LogTransaction("RemoveChild", child.Id, null, null, child.SpeckleBranchName, child.SpeckleBranchId, "Failed");
                                }    
                        }
                    }
                    await _unitOfWork.CompleteAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                     transaction.Rollback();
                     _logger.LogError(ex, "Error during DeleteModel");
                     await LogTransaction("DeleteModel", id, null, null, null, null, "Failed");
                     throw ex;
                }
            });
        }

        public async Task<Entity.CSMS_Entity.Model> Update(UpdateModelDTO updateDto, string userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                try
                {
                    var entity = GetModelById(updateDto.Id);
                    if (entity == null) throw new ArgumentException("Không tìm thấy bản ghi để cập nhật");

                    var user = GetUserById(userId);
                    if (user == null) throw new ArgumentException("User không tồn tại!");

                    var project = GetProjectById(entity.ProjectID);
                    if (project == null) throw new ArgumentException("Project không tồn tại!");

                        if (entity.SpeckleBranchId != null)
                        {
                            await UpdateBranch(entity.SpeckleBranchId, updateDto.Name, project.SpeckleProjectId, updateDto.Description);
                        }
                    entity.Name = updateDto.Name;
                    entity.Description = updateDto.Description;
                    entity.SetValueUpdate(user.UserName);

                    _context.Models.Update(entity);
         
                    await _unitOfWork.CompleteAsync();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during UpdateModel");
                        await LogTransaction("UpdateModel", updateDto.Id, null, null, null, null, "Failed");
                        throw;
                    }
            });
            
        }
        public async Task<Entity.CSMS_Entity.Model> MoveModel(MoveModelDTO dto, string userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        var user = GetUserById(userId);
                        var model = GetModelById(dto.Id);
                        if (model == null) throw new ArgumentException("Model không tồn tại!");

                        var project = GetProjectById(model.ProjectID);
                        if (project == null) throw new ArgumentException("Project không tồn tại!");

                        var oldParent = GetModelById(model.ParentId);
                        var newParent = GetModelById(dto.NewParentId);

                        if (newParent != null && newParent.IsUpload) throw new ArgumentException("Model đã có file, không thể di chuyển tới!");

                        var children = GetAllChildren(model.Id);
                        List<Entity.CSMS_Entity.Model> remainingChildren = null;                 
                        if (oldParent != null)
                        {
                           remainingChildren = _context.Models.Where(m => m.ParentId == oldParent.Id && m.Id != model.Id).ToList();
                        }
                        
                        var newParentChildren = GetAllChildren(newParent?.Id);

                        Task speckleTask;
                        if (newParent == null && remainingChildren == null)
                        {
                            //Không có cha mới(lv1) và cha cũ không có con
                            if (oldParent != null)
                            {
                                oldParent.Type = "MODEL";
                                await CreateBranch(oldParent.Name, project.SpeckleProjectId, oldParent.Description)
                                .ContinueWith(task =>
                                {
                                    if (task.Exception != null) throw task.Exception;
                                    oldParent.SpeckleBranchId = task.Result;
                                });
                            }

                            speckleTask = Task.CompletedTask;
                        }
                        else if (newParent != null && remainingChildren?.Count > 0)
                        {
                            if (!newParentChildren.Any())
                            {
                                await DeleteBranch(newParent.SpeckleBranchId, project.SpeckleProjectId)
                                .ContinueWith(task =>
                                {
                                    newParent.SpeckleBranchId = null;
                                    newParent.Type = "FOLDER";
                                    newParent.SetValueUpdate(user.UserName);
                                });
                            }
                            speckleTask = Task.CompletedTask;
                        }
                        else if (newParent != null && remainingChildren == null)
                        {
                            //Cha mới tồn tại và cha cũ không có con
                            if (oldParent != null)
                            {
                                oldParent.Type = "MODEL";
                                await CreateBranch(oldParent.Name, project.SpeckleProjectId, oldParent.Description)
                                .ContinueWith(task =>
                                {
                                    if (task.Exception != null) throw task.Exception;
                                    oldParent.SpeckleBranchId = task.Result;
                                });
                            }

                            if (!newParentChildren.Any())
                            {
                                await DeleteBranch(newParent.SpeckleBranchId, project.SpeckleProjectId)
                                .ContinueWith(task =>
                                {
                                    newParent.SpeckleBranchId = null;
                                    newParent.Type = "FOLDER";
                                    newParent.SetValueUpdate(user.UserName);
                                });
                             }
                            speckleTask = Task.CompletedTask;
                        }
                        else
                        {
                            speckleTask = Task.CompletedTask;
                        }
                        await speckleTask;          
                        
                        if (oldParent != null) _context.Models.Update(oldParent);
                        if (newParent != null) _context.Models.Update(newParent);

                        try
                        {
                            model.Level = newParent != null ? newParent.Level + 1 : 1;
                            model.ParentId = newParent != null ? newParent.Id : null;
                            model.SetValueUpdate(user.UserName);
                            _context.Models.Update(model);
                            
                            if (children.Any())
                            {
                                foreach (var child in children)
                                {
                                    try
                                    {
                                        var parent = _context.Models.FirstOrDefault(m => m.Id == child.ParentId);
                                        child.Level = parent.Level + 1;
                                        child.SetValueUpdate(user.UserName);
                                        _context.Models.Update(child);
                                    }
                                    catch (Exception updateChildEx)
                                    {
                                        _logger.LogError(updateChildEx, "Error during updating model");
                                        await LogTransaction("UpdateChild", child.Id, null, null, child.SpeckleBranchName, child.SpeckleBranchId, "Failed");
                                    }
                                }
                            }

                            await _unitOfWork.CompleteAsync();
                            transaction.Commit();
                            return model;
                        }
                        catch (Exception updateEx)
                        {
                            _logger.LogError(updateEx, "Error during updating model");
                            await LogTransaction("UpdateModel", model.Id, oldParent?.Id, newParent?.Id, null, oldParent?.SpeckleBranchId, "Failed");
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error during MoveModel");
                        await LogTransaction("MoveModel", dto.Id, null, null, null, null, "Failed");
                        throw;
                    }
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
        
        private List<Entity.CSMS_Entity.Model> GetAllChildren(string parentId)
        {
            var children = new List<Entity.CSMS_Entity.Model>();
            var stack = new Stack<Entity.CSMS_Entity.Model>();
            var visited = new HashSet<string>(); // Tập hợp để theo dõi các node đã truy xuất

            var parent = _context.Models.FirstOrDefault(m => m.Id == parentId);
            if (parent != null)
            {
                stack.Push(parent);
                visited.Add(parent.Id);
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                children.Add(current);

                var directChildren = _context.Models.Where(m => m.ParentId == current.Id).ToList();
                foreach (var child in directChildren)
                {
                    // Chỉ thêm vào stack nếu chưa được truy xuất trước đó
                    if (!visited.Contains(child.Id))
                    {
                        stack.Push(child);
                        visited.Add(child.Id);
                    }
                }
            }
            // Loại bỏ phần tử gốc nếu không muốn bao gồm nó trong danh sách kết quả
            if (children.Any() && children[0].Id == parentId)
            {
                children.RemoveAt(0);
            }

            return children;
        }

        private async Task UpdateBranch(string id, string name, string streamId, string description)
        {
            var result = await SpeckleHelper.UpdateBranch(id, name, streamId, description);
            if (!result)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật nhánh!");
            }
        }

        private async Task DeleteBranch(string branchId, string streamId)
        {
            var result = await SpeckleHelper.DeleteBranch(branchId, streamId);
            if (!result)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xóa nhánh!");
            }
        }

        private async Task<string> CreateBranch(string branchName, string streamId, string description)
        {
            var speckleId = await SpeckleHelper.CreateBranch(
                        branchName,
                        streamId,
                        description);
            if (speckleId == null)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình tạo nhánh!");
            }
            return speckleId;
        }

        private Entity.CSMS_Entity.Model GetModelById(string id)
        {
            var model = _context.Models.FirstOrDefault(m => m.Id == id);            
            return model;
        }

        private Project GetProjectById(string id)
        {
            var project = _context.Projects.Where(p => p.Id == id).FirstOrDefault();
            if (project == null)
            {
                throw new Exception("Project không tồn tại!");
            }
            return project;
        }


        private CSMS.Entity.IdentityAccess.User GetUserById(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("User không tồn tại!");
            }
            return user;
        }

        public async Task<List<Entity.CSMS_Entity.Model>> GetTreeModelByParentId(string[] parentIds, bool includeUploaded)
        {
            try
            {
                // Lấy tất cả các model mà có Id nằm trong parentIds
                var parentModels = await _context.Models
                    .Include(i => i.Project)
                    .Where(x => parentIds.Contains(x.Id))
                    .ToListAsync();

                if (!includeUploaded)
                {
                    parentModels = parentModels.Where(x => !x.IsUpload).ToList();
                }

                // Lấy tất cả các model con của các parentModels
                var allParentIds = parentModels.Select(m => m.Id).ToList();
                var childModels = await _context.Models
                    .Include(i => i.Project)
                    .Where(x => allParentIds.Contains(x.ParentId))
                    .ToListAsync();

                // Kết hợp các model cha và con
                var allModels = parentModels.Concat(childModels).ToList();

                // Lặp lại để lấy tất cả các model con của các childModels
                var childIds = childModels.Select(c => c.Id).ToList();
                while (childIds.Any())
                {
                    var grandChildModels = await _context.Models
                        .Include(i => i.Project)
                        .Where(x => childIds.Contains(x.ParentId))
                        .ToListAsync();

                    if (!includeUploaded)
                    {
                        grandChildModels = grandChildModels.Where(x => !x.IsUpload).ToList();
                    }

                    allModels.AddRange(grandChildModels);
                    childIds = grandChildModels.Select(c => c.Id).ToList();
                }

                if (!allModels.Any()) throw new ArgumentException("Không có bản ghi nào tồn tại!");

                return allModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IQueryable<Entity.CSMS_Entity.Model> GetDirectChildrenByParentId(string parentId)
        {
            return _context.Models.Where(m => m.ParentId == parentId && m.Level == m.Parent.Level + 1);
        }

        public IQueryable<Entity.CSMS_Entity.Model> GetModelByProjectId(string projectId)
        {
            return _context.Models.Where(m => m.ProjectID == projectId && m.Level == 1);
        }

        public async Task<List<Entity.CSMS_Entity.Model>> GetTreeModelByProjectId(string projectId, bool includeUploaded)
        {
            try
            {
                var result = await _context.Models
                    .Include(i => i.Project)
                    .Where(x => x.ProjectID == projectId)
                    .ToListAsync();
                if (!includeUploaded)
                {
                    result = result.Where(x => !x.IsUpload).ToList();
                }
                if (!result.Any()) throw new ArgumentException("Không có bản ghi nào tồn tại!");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<SpeckleUploadResult> UploadFileIFC(string modelId, string filePath, string userId)
        {
            try
            {
                var model = GetModelById(modelId);
                if (model == null) throw new ArgumentException("Model không tồn tại!");

                var project = GetProjectById(model.ProjectID);
                var user = GetUserById(userId);
                var result = await SpeckleHelper.UploadFileIFC(project.SpeckleProjectId, model.Name, filePath);

                if (result == null) throw new ArgumentException("Không thể upload file lên model!");
              
                var modelVersion = new ModelVersion
                {
                    ObjectId = result.ObjectId,
                    CommitId = result.CommitId,
                    ModelId = model.Id,
                    BranchName = result.BranchName
                };
                if (model.IsUpload)
                {
                    var oldVersion = _context.ModelVersions.FirstOrDefault(m => m.ModelId == model.Id);
                    model.CreatedBy = oldVersion.CreatedBy;
                    model.CreatedDate = oldVersion.CreatedDate;
                    modelVersion.SetValueUpdate(user.UserName);                   
                }
                else
                {
                    modelVersion.SetDefaultValue(user.UserName);
                    modelVersion.SetValueUpdate(user.UserName);
                    model.IsUpload = true;
                    _context.Models.Update(model);
                }
                
                _context.ModelVersions.Add(modelVersion);               
                _context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ModelVersionDTO> GetModelWithVersionsAsync(string modelId)
        {
            try
            {
                var model = await _context.Models
                    .Include(m => m.Project)
                    .Include(m => m.ModelVersion)
                    .Where(m => m.Id == modelId)
                    .FirstOrDefaultAsync();
                if (model == null) throw new ArgumentException("Không có bản ghi nào tồn tại!");

                var project = GetProjectById(model.ProjectID);


                var modelVersionDTO = new ModelVersionDTO
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
                    ModifiedDate = model.ModifiedDate,
                    Versions = new List<VersionDTO>()
                };

                var sortedVersions = model.ModelVersion.OrderByDescending(v => v.CreatedDate).ToList();

                for (int i = 0; i < sortedVersions.Count; i++)
                {
                    var version = sortedVersions[i];
                    var commit = await SpeckleHelper.GetCommit(project.SpeckleProjectId, version.CommitId);

                    var versionDTO = new VersionDTO
                    {
                        Id = version.Id,
                        ObjectId = version.ObjectId,
                        CommitId = version.CommitId,
                        BranchName = version.BranchName,
                        ModelId = version.ModelId,
                        ModelName = version.Model.Name,
                        CommitMessage = commit.message,
                        CreatedBy = version.CreatedBy,
                        CreatedDate = version.CreatedDate,
                        ModifiedBy = version.ModifiedBy,
                        ModifiedDate = version.ModifiedDate,
                        UploadOrder = i + 1 // Thiết lập thứ tự upload
                    };
                    modelVersionDTO.Versions.Add(versionDTO);
                }

                return modelVersionDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Dictionary<string, ModelVersion>> GetLatestVersionsByModelIdsAsync(List<string> modelIds)
        {
            return await _context.ModelVersions
                .Where(v => modelIds.Contains(v.ModelId))
                .GroupBy(v => v.ModelId)
                .Select(g => new
                {
                    ModelId = g.Key,
                    LatestVersion = g.OrderByDescending(v => v.CreatedDate).FirstOrDefault()
                })
                .ToDictionaryAsync(x => x.ModelId, x => x.LatestVersion);
        }

        public async Task<List<SpeckleModelInfo>> GetSpeckleModelInfo(List<string> modelIds)
        {
            var result = await _context.Models
                .Where(m => modelIds.Contains(m.Id))
                .Include(m => m.ModelVersion)
                .Select(m => new SpeckleModelInfo
                {
                    Id = m.Id,
                    Name = m.Name,
                    SpeckleModelId = m.SpeckleBranchId,
                    SpeckleVersionInfos = m.ModelVersion
                        .OrderByDescending(v => v.ModifiedDate)
                        .Select(v => new SpeckleVersionInfo
                        {
                            Id = v.Id,
                            CommitId = v.CommitId,
                            ObjectId= v.ObjectId,
                            ModifiedDate = v.ModifiedDate,
                        }).ToArray()
                })
                .ToListAsync();

            return result;
        }

    }
    
}

