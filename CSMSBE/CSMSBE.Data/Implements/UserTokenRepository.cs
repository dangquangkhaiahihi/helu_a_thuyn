
using CSMS.Entity;
using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Helper;
using CSMS.Data.Repository;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Data.Implements
{
    public class UserTokenRepository : BaseRepository<UserTokens>, IUserTokenRepository
    {
        private readonly ILogger<UserTokenRepository> _logger;

        public UserTokenRepository(csms_dbContext context, ILogger<UserTokenRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<bool> ConcurrencyLogin(string userId, string device, string userType)
        {
            try
            {
                if (userType == Constant.UserType.REGISTERUSER)
                {
                    var userToken = await Dbset.SingleOrDefaultAsync(c => c.UserId == userId
                                && c.Name == device);

                    if (userToken == null) return true;
                    if (DateTimeOffset.UtcNow <= userToken.ExpiredTime.Value) return false;
                    else
                    {
                        return true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Đã có lỗi xảy ra trong quá trình check tồn tại access token", ex);
                return false;
            }

        }

        public async Task<bool> IsExistAccessToken(string userId, string device, string accessToken)
        {
            try
            {
                var userToken = await Dbset.SingleOrDefaultAsync(c => c.UserId == userId
                                 && c.Name == device && c.Value == accessToken);

                if (userToken == null) return false;

                //if (DateTime.Now <= userToken.ExpiredTime.Value) return true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Đã có lỗi xảy ra trong quá trình check tồn tại access token", ex);
                return false;
            }
        }

    }
}
