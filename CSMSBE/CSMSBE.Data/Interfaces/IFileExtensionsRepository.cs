using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IFileExtensionsRepository
    {
        Task<FileExtensions> GetByName(string name);
        Task<List<FileExtensions>> GetAll();
        IQueryable<FileExtensions> GetLookupFileExtension(IKeywordDto keywordDto);
    }
}
