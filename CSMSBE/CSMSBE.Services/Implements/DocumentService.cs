using AutoMapper;
using Castle.Core.Logging;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.Document;
using CSMSBE.Core.Helper;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Model.Notification;

namespace CSMSBE.Services.Implements
{
    public class DocumentService : IDocumentService
    {
        private readonly string _folderRootPath;
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileExtensionsRepository _fileExtensionsRepository;
        private readonly ILogger<DocumentService> _logger;
        private readonly csms_dbContext _csms_dbContext;
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository documentRepository, IMapper mapper, IFileExtensionsRepository fileExtensionsRepository, string folderRootPath
            , csms_dbContext csms_DbContext, ILogger<DocumentService> logger)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _fileExtensionsRepository = fileExtensionsRepository;
            _folderRootPath = folderRootPath;
            _csms_dbContext = csms_DbContext;
            _logger = logger;
        }
        private string GenerateUniqueDocumentName(string baseName, string projectId, int parentId)
        {
            var existingDocuments = _csms_dbContext.Documents
                .Where(d => d.ProjectId == projectId && d.Name.StartsWith(baseName) && d.ParentId == parentId)
                .ToList();

            if (!existingDocuments.Any())
            {
                return baseName;
            }

            var existingNames = existingDocuments
                .Select(d => d.Name)
                .Where(name => name.StartsWith(baseName))
                .Select(name => {
                    var suffix = name.Substring(baseName.Length).Trim('(', ')');
                    return int.TryParse(suffix, out var number) ? number : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            var newSuffix = existingNames + 1;
            return $"{baseName}({newSuffix})";
        }
        public string GetSubstringAfterSecondSlash(string path)
        {
            var segments = path.Split('/');
            return segments.Skip(2).Any() ? string.Join("/", segments.Skip(2)) : path;
        }
        public async Task<bool> IsDocumentFileAllowed(DocumentCreateDto dto)
        {
            try
            {
                if (dto.IsFile == true)
                {
                    if (dto.File == null && dto.File.Length == 0)
                    {
                        throw new Exception("Không có file nào được upload lên");
                    }
                    var listExtenstion = await _fileExtensionsRepository.GetAll();
                    if (listExtenstion.Any(x => x.Name.Equals(Path.GetExtension(dto.File.FileName))))
                    {
                        return true;
                    }
                    else return false;
                }
                else
                {
                    if (dto.File != null)
                    {
                        throw new Exception("Không thể upload file khi tạo mới folder");
                    }
                    else return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }

        public async Task<DocumentDto> CreateDocument(DocumentCreateDto dto, string userId)
        {
            try
            {
                var entity = _mapper.Map<Document>(dto);

                var parent = await _documentRepository.GetDocumentById(dto.ParentId.GetValueOrDefault());
                if(parent == null)
                {
                    throw new Exception("Không tìm thấy document cha");
                }
                else if(parent.IsFile)
                {
                    throw new Exception("Không thể tạo document con trong file");
                }
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{dto.ProjectId}");
                if (!dto.IsFile)
                {
                    entity.Name = GenerateUniqueDocumentName(entity.Name, dto.ProjectId, parent.Id);
                    var createResult = await _documentRepository.CreateDocument(entity, userId);
                   
                    return _mapper.Map<DocumentDto>(createResult);
                }
                else
                {
                    var saveResult = await FileHelper.SaveFileAsync(dto.File, folderPath);
                    var extension = Path.GetExtension(dto.File.FileName);
                    /*var fileExtension = await _fileExtensionsRepository.GetByName(extension);*/

                    entity.Size = dto.File.Length;
                    entity.Name = dto.File.FileName;
                    entity.FileExtension = extension;

                    entity.UrlPath = $"uploads/project_{dto.ProjectId}/{entity.Name}";
                    var createResult = await _documentRepository.CreateDocument(entity, userId);
                    return _mapper.Map<DocumentDto>(createResult);
                }
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileResult> DownloadFile(int documentId)
        {
            var entity = await _documentRepository.GetDocumentById(documentId);
            if (entity == null)
            {
                throw new Exception("Document không tồn tại!");
            }

            if (!entity.IsFile)
            {
                throw new Exception("Document không phải là file!");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.UrlPath.TrimStart('/'));

            try
            {
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                var fileType = FileHelper.GetContentType(filePath);
                var fileContentResult = new FileContentResult(fileBytes, fileType)
                {
                    FileDownloadName = entity.Name
                };
                return fileContentResult;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentDto> GetDocumentById(int id)
        {
            try
            {
                var result = await _documentRepository.GetDocumentById(id);
                return _mapper.Map<DocumentDto>(result); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentDto> GetAllDocumentByProjectId(string projectId)
        {
            try
            {
                var result = await _documentRepository.GetDocumentTreeByProjectId(projectId);
                return _mapper.Map<DocumentDto>(result); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentDto> MoveDocument(int documentId, int destinationId)
        {
            try
            {
                var entity = await _documentRepository.GetDocumentById(destinationId);
                if (entity == null)
                {
                    throw new Exception($"Document chuyển tới không tồn tại");
                }
                else if (entity.IsFile)
                {
                    throw new Exception($"Không thể di chuyển document vào trong file");
                }
                
                var moveResult = await _documentRepository.MoveDocument(documentId, destinationId);
                 var dto = _mapper.Map<DocumentDto>(moveResult);
                return dto;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RemoveFileRecursive(Document document)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{document.ProjectId}");
            if (document.IsFile)
            {
                folderPath = $"{folderPath}/{document.Name}";
                FileHelper.DeleteFile(folderPath);
            }
            else
            {
                if (!document.ParentId.HasValue)
                {
                    FileHelper.DeleteFile($"{folderPath}");
                }
                if(document.Children != null && document.Children.Any())
                {
                    foreach(var child in document.Children)
                    {
                        RemoveFileRecursive(child);
                    }
                }
                else
                {
                    return;
                }
            }
        }
        public async Task RemoveDocuments(int[] ids)
        {
            try
            {
                foreach(int id in ids)
                {
                    var entity = await _documentRepository.GetTreeDocumentById(id);
                    if (entity == null)
                    {
                        continue;
                    }
                    RemoveFileRecursive(entity);
                    await _documentRepository.RemoveDocument(entity);
                }  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentDto> UpdateDocument(DocumentUpdateDto dto, string userId)
        {
            try
            {
                var entity = _mapper.Map<Document>(dto);
                var updateResult = await _documentRepository.UpdateDocument(entity, userId);
                return _mapper.Map<DocumentDto>(updateResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocumentDto>> GetByParentId(int id)
        {
            try
            {
                var listDocument = await _documentRepository.GetByParentId(id);
                return _mapper.Map<List<DocumentDto>>(listDocument);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileResult> PreviewDocument(string filePath)
        {
            return await FileHelper.PreviewFileAsync(filePath);
        }

        public async Task<DocumentDto> GetInitDocumentTree(string projectId)
        {
            try
            {
                var root = await _documentRepository.GetRootDocumentByProjectId(projectId);
                return _mapper.Map<DocumentDto>(root);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentDto> ReviewDocument(int id, string status, string userId)
        {
            try
            {
                if(!status.Equals("REJECTED") && !status.Equals("APPROVED"))
                {
                    throw new Exception("Trạng thái review không hợp lệ");
                }
                var document = await _documentRepository.GetDocumentById(id);
                if (!document.IsFile)
                {
                    throw new Exception("Chỉ có thể review file!");
                }
                var documentReivewResult = await _documentRepository.ReviewDocument(id,status, userId);
                return _mapper.Map<DocumentDto>(documentReivewResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<LookupDTO>> GetLookUpFileExtension(IKeywordDto keywordDto)
        {
            try
            {
                var fileExtensions = _fileExtensionsRepository.GetLookupFileExtension(keywordDto);
                if (fileExtensions != null)
                {
                    var lookupFileExtensionDto = await Task.Run(() => fileExtensions.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupFileExtensionDto;
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
