using CSMS.Entity.IdentityAccess;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.UserDTO;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.UserDTO;
using CSMS.Model.IdentityAccess;
using CSMS.Model.User;
using CSMSBE.Core.Interfaces;
using CSMSBE.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Model.DTO.Document;

namespace CSMSBE.Services.Interfaces
{
    public interface IUserService
    {
        User GetByUserid(string id);
        string GetRoleByUserId(string id);
        DetailUserDTO GetUserDetail(string id);
        public string GetCurrentUserId();
        public string GetCurrentUserName();
        Task<bool> KickOutUser(string userId);
        Task<bool> UserLogin(User user, ExternalLoginInfor externalLoginInfor, UserLoginInfo userInfo);
        string GetUserClaimByUserId(string id);
        Task<IList<UserLookup>> GetLookUpUsers(IKeywordDto keywordDto);
        Task<IPagedList<UserDTO>> FilterUsers(UserFilterRequest filter);
        UserDTO UpdateUser(UpdateUserDTO model);
        Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleId);
        Task<bool> UploadAvatar(UploadAvatarDTO dto);
    }
}
