using CSMS.Entity.IdentityAccess;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserTokenRepository : IBaseRepository<UserTokens>
    {
        Task<bool> ConcurrencyLogin(string userId, string device, string userType);
        Task<bool> IsExistAccessToken(string userId, string device, string accessToken);
    }
}
