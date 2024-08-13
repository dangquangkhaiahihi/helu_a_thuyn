using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.Document;
using CSMS.Model.DTO.SomeTableDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDto> CreateDocument(DocumentCreateDto dto, string userId);
        Task<bool> IsDocumentFileAllowed(DocumentCreateDto dto);
        Task<DocumentDto> GetDocumentById(int id);
        Task<FileResult> DownloadFile(int documentId);
        Task<FileResult> PreviewDocument(string filePath);
        Task<DocumentDto> MoveDocument(int documentId, int destinationId);
        Task RemoveDocuments(int[] ids);
        Task<DocumentDto> ReviewDocument(int id, string status, string userId);
        Task<DocumentDto> GetAllDocumentByProjectId(string projectId);
        Task<DocumentDto> GetInitDocumentTree(string projectId);
        Task<List<DocumentDto>> GetByParentId(int id);
        Task<DocumentDto> UpdateDocument(DocumentUpdateDto dto, string userId);
        Task<IList<LookupDTO>> GetLookUpFileExtension(IKeywordDto keywordDto);
    }
}
