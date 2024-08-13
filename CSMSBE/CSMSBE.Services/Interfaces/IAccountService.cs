using CSMS.Entity.IdentityExtensions.IdentityMapping;
using CSMS.Model;
using CSMS.Model.IdentityAccess;
using CSMS.Model.User;
using CSMSBE.Core;
using CSMS.Model.IdentityAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public interface IAccountService
    {
        public Task<string> LogInAsync(string email);
        public Task InsertUserLoginLog(UserApiLogRequest apiLog);
        public Task InsertRefreshTokens(string userName, string refreshToken, string RemoteIpAddress);
        public string GenerateRefreshToken();
        public Task DeleteRefreshTokens(string userName);
        public Task<ResponseData> ExchangeRefreshToken(ExchangeRefreshTokenRequest message, string device, string v);
        public Task DeleteRefreshTokensByUserName(string userName, string device);
        public Task LogOut(CurrentUserDTO user, string refreshTokenPr, string uUid);
    }
}
