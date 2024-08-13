using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetDocumentById(int id);
        Task<Document> GetRootDocumentByProjectId(string projectId);
        Task<Document> CreateDocument(Document entity, string userId);
        Task RemoveDocument(Document document);
        Task<Document> MoveDocument(int id, int destId);
        Task<Document> UpdateDocument(Document entity, string userId);
        Task<List<Document>> GetByParentId(int documentId);
        Task<Document> GetDocumentTreeByProjectId(string projectId);
        Task<Document> GetTreeDocumentById(int id);
        Task<Document> ReviewDocument(int id, string status, string userId);
    }
}
