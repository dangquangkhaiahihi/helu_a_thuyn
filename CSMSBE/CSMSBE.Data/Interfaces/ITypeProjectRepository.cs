using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;

namespace CSMS.Data.Interfaces
{
    public interface ITypeProjectRepository
    {
        IQueryable<TypeProject> GetLookupTypeProject (IKeywordDto keywordDto);
    }
}
