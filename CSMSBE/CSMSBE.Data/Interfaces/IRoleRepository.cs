using CSMS.Entity.IdentityAccess;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Role;

namespace CSMS.Data.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        IQueryable<Role> GetLookupRole(IKeywordDto keywordDto);
        Role Get(string id);
        Role Create(CreateRoleDTO table, string userId);
        Role Update(UpdateRoleDTO table, string userId);
        bool Remove(string id);
    }
}
