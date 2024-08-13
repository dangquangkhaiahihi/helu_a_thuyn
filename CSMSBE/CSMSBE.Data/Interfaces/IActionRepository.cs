using CSMS.Data.Repository;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IActionRepository : IBaseRepository<Entity.SecurityMatrix.Action>
    {
        IQueryable<Entity.SecurityMatrix.Action> GetLookUpAction(IKeywordDto keywordDto);
    }
}
