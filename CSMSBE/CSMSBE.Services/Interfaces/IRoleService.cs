using CSMS.Entity.IdentityAccess;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Role;
using CSMS.Model.User;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Model.DTO.FilterRequest;

namespace CSMSBE.Services.Interfaces
{
    public interface IRoleService
    {
        Role GetById(string id);
        bool GetByCode(string code);
        RoleDTO CreateRole(CreateRoleDTO dto, string userId);
        RoleDTO UpdateRole(UpdateRoleDTO dto, string userId);
        bool RemoveRole(string id);
        string GetRoleNameById(string id);
        RoleDTO GetRoleById(string id);
        bool CheckRoleById(string id, string roleCode);
        Task<IList<RoleDTO>> GetLookupRole(IKeywordDto keywordDto);
        Task<IPagedList<RoleDTO>> FilterRole(RoleFilterRequest filter);
    }
}
