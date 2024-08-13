using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Model.DTO.Document;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<DocumentRepository> _logger;
        private readonly IMapper _mapper;
        public DocumentRepository(csms_dbContext context, ILogger<DocumentRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        private User GetCurrentUser(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) throw new ArgumentException("User không tồn tại!");
            return user;
        }
        public async Task<Document> GetTreeDocumentById(int id)
        {
            try
            {
                var entity = await _context.Documents.Include(x => x.Children).ThenInclude(child => child.Children).FirstOrDefaultAsync(x => x.Id == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<Document> GetRootDocumentByProjectId(string projectId)
        {
            try
            {
                var entity = await _context.Documents
                    .Include(x => x.Children)
                    .Where(x => x.ProjectId == projectId && !x.ParentId.HasValue)
                    .Select(root => new Document
                    {
                        Id = root.Id,
                        Name = root.Name + root.FileExtension,
                        IsFile = root.IsFile,
                        Status = root.Status,
                        UrlPath = root.UrlPath,
                        Size = root.Size,
                        FileExtension = root.FileExtension,
                        ProjectId = root.ProjectId,
                        Project = root.Project,
                        Children = root.Children.Select(child => new Document
                        {
                            Id = child.Id,
                            Name = child.Name + child.FileExtension,
                            IsFile = child.IsFile,
                            Status = child.Status,
                            UrlPath = child.UrlPath,
                            Size = child.Size,
                            FileExtension = child.FileExtension,
                            ProjectId = child.ProjectId,
                            Project = child.Project,
                            ParentId = child.ParentId,
                            Parent = null, // No need to include the parent here
                            Children = null // Set Children to null to exclude grandchildren
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Document> GetDocumentById(int id)
        {
            try
            {
                var document = await _context.Documents.FindAsync(id);
                return document;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> CreateDocument(Document entity, string userId)
        {
            try
            {
                var user = GetCurrentUser(userId);
                entity.SetDefaultValue(user.UserName);
                entity.SetValueUpdate(user.UserName);
                var createResult = await _context.Documents.AddAsync(entity);
                await _context.SaveChangesAsync();
                return createResult.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public async Task RemoveDocument(Document document)
        {
            try
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> MoveDocument(int id, int destId)
        {
            try
            {
                var parent = await _context.Documents.FindAsync(destId);
                var entity = await _context.Documents.FindAsync(id);
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{entity.ProjectId}", entity.Name);
                var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{parent.ProjectId}", entity.Name);

                if (File.Exists(oldFilePath))
                {
                    File.Move(oldFilePath, newFilePath);
                }
                if (!string.IsNullOrEmpty(entity.UrlPath)) entity.UrlPath = $"uploads/project_{parent.ProjectId}/{entity.Name}";
                
                entity.ParentId = destId;
                _context.Documents.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetSubstringBeforeLastSlash(string path)
        {
            int lastSlashIndex = path.LastIndexOf('/');
            if (lastSlashIndex == -1)
            {
                return path;
            }
            return path.Substring(0, lastSlashIndex);
        }

        public async Task<Document> UpdateDocument(Document updatedEntity, string userId)
        {
            try
            {
                var user = GetCurrentUser(userId);
                var entity = _context.Documents.Find(updatedEntity.Id);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy tài liệu");
                }
                if (entity.Name != updatedEntity.Name)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{entity.ProjectId}");
                    var oldFilePath = Path.Combine(filePath, entity.Name);
                    var newFilePath = Path.Combine(filePath, updatedEntity.Name);
                    if (File.Exists(oldFilePath))
                    {
                        File.Move(oldFilePath, newFilePath);
                    }
                }

                entity.Name = updatedEntity.Name;
                if (!string.IsNullOrEmpty(entity.UrlPath)) entity.UrlPath = $"{GetSubstringBeforeLastSlash(entity.UrlPath)}/{entity.Name}";
                entity.SetValueUpdate(user.UserName);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> GetDocumentTreeByProjectId(string projectId)
        {
            try
            {
                var entity = await _context.Documents.Include(x => x.Children).ThenInclude(child => child.Children).FirstOrDefaultAsync(x => x.ProjectId == projectId && !x.ParentId.HasValue);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<Document>> GetByParentId(int documentId)
        {
            try
            {
                var listDocument = await _context.Documents
                    .Where(x => x.ParentId == documentId)
                    .Select(child => new Document
                    {
                        Id = child.Id,
                        Name = child.Name + child.FileExtension,
                        IsFile = child.IsFile,
                        Status = child.Status,
                        UrlPath = child.UrlPath,
                        Size = child.Size,
                        FileExtension = child.FileExtension,
                        ProjectId = child.ProjectId,
                        Project = child.Project,
                        ParentId = child.ParentId,
                        Parent = null, // No need to include the parent here
                        Children = null // Set Children to null to exclude grandchildren
                    })
                    .ToListAsync();

                return listDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> ReviewDocument(int id, string status, string userId)
        {

            try
            {
                var user = GetCurrentUser(userId);
                var document = await _context.Documents.FindAsync(id);
                document.SetValueUpdate(user.UserName);
                document.Status = status;
                await _context.SaveChangesAsync();
                return document;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
