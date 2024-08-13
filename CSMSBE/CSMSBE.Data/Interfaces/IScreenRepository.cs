using CSMS.Data.Repository;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IScreenRepository : IBaseRepository<Screen>
    {
        IQueryable<Screen> GetLookUpScreen(IKeywordDto keywordDto);
    }
}
