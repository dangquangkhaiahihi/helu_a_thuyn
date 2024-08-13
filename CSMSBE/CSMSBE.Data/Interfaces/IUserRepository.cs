using CSMS.Entity.IdentityAccess;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.User;
using CSMS.Model.DTO.BaseFilterRequest;

namespace CSMS.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User UpdateUser(UpdateUserDTO updateDto);
        Task<User> Create(CreateUserDTO createdto);
        List<UserRole> GetListUserRole(string RoleId);
        Role GetRoleDetail(string roleId);
        IQueryable<User> GetLookupUser(IKeywordDto keywordDto);
        Task<bool> UploadAvatar (string avatarUrl, string userId);
        Task<User> GetUserById(string id);
    }
}
