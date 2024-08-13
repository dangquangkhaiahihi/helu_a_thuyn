using AutoMapper.Configuration;
using CSMS.Entity.IdentityAccess;
using CSMS.Model.IdentityAccess;
using CSMS.Model.IdentityAccess;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Services.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Options;
using CSMS.Entity;
using CSMS.Entity.IdentityExtensions;
using CSMS.Data.Interfaces;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Core.Helper;
using Data.Interfaces;
using CSMS.Entity.IdentityExtensions.IdentityMapping;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using Microsoft.EntityFrameworkCore;
using CSMSBE.Core.Interfaces;
using CSMS.Model.User;
using CSMS.Model;
using CSMSBE.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using CSMS.Data.Repository;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using CSMSBE.Model.Repository;

namespace CSMSBE.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly csms_dbContext _csms_DbContext;
        private readonly UserManager<User> _userManager;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;
        private readonly IAspNetRefreshTokensRepository _refreshTokensRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IUserService _userService;
        private readonly IRepository<UserLoginLog, int> _userLoginLog;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager,
            IOptions<AuthenticationSettings> authenticationSettings,
            ILogger<AccountService> logger,
            IAspNetRefreshTokensRepository refreshTokensRepository,
            IUnitOfWork unitOfWork,
            IRepository<UserLoginLog, int> userLoginLog,
            IUserTokenRepository userTokenRepository,
            IUserService userService,
            JwtSecurityTokenHandler jwtSecurityTokenHandler,
            csms_dbContext csms_DbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _authenticationSettings = authenticationSettings.Value;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _refreshTokensRepository = refreshTokensRepository;
            _userTokenRepository = userTokenRepository;
            _userLoginLog = userLoginLog;
            _userService = userService;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _csms_DbContext = csms_DbContext;
            _httpContextAccessor = httpContextAccessor;
        }

       
        public async Task<string> LogInAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(email);
            }
            var userRoles = _userService.GetRoleByUserId(user.Id);

            var expireTime = DateTime.Now.AddMinutes(30);
            var key = Encoding.UTF8.GetBytes(_authenticationSettings.SecretKey);
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwt = new JwtSecurityToken(
                null,
                null,
                claims: GetTokenClaims(user, userRoles ?? string.Empty, expireTime),
                expires: expireTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = tokenHandler.WriteToken(jwt);
            return accessToken;
        }
        private IEnumerable<Claim> GetTokenClaims(User user, 
            string role, 
            DateTime expireTime)
        {
            return new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
             new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
             new Claim("fullName", user.FullName ?? string.Empty),
             new Claim(ClaimTypes.Email, user.Email?? string.Empty),
             new Claim(ClaimTypes.Role, role ?? string.Empty),
             new Claim(ClaimTypes.Expired, expireTime.ToString()),
         };
        }
        public string GenerateRefreshToken()
        {
            int size = 32;
            var randomNumber = new byte[size];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task InsertRefreshTokens(string UserName, 
           string refreshToken,
           string remoteIpAddress)
        {
            try
            {
                var refreshTokenOlds = _refreshTokensRepository.GetAll().Where(c => c.UserName == UserName 
                && c.RemoteIpAddress == remoteIpAddress).ToList();

                if (refreshTokenOlds.Any())
                {
                    _refreshTokensRepository.DeleteMulti(refreshTokenOlds);
                }

                var token = new AspNetRefreshTokens
                {
                    UserName = UserName,
                    Token = refreshToken,
                    Expires = DateTime.Now.AddDays(Convert.ToInt32(_authenticationSettings.RefreshExpireDay)),
                    RemoteIpAddress = remoteIpAddress,
                    CreatedBy = UserName,
                    CreatedDate = DateTimeOffset.Now,
                    ModifiedBy = UserName,
                    ModifiedDate = DateTimeOffset.Now,
                    IsDelete = false
                };
                _refreshTokensRepository.Insert(token);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra trong quá trình DeleteRefreshTokensByUserName");
            }

        }
        public async Task InsertUserToken(string userId,
            string deviceType,
            string accessToken,
            string userType,
            DateTime expiredTime)
        {
            if (string.IsNullOrEmpty(deviceType))
            {
                deviceType = Constant.DeviceDefault;
            }
            var uuId = Guid.NewGuid().ToString();
            if (userType == Constant.UserType.REGISTERUSER)
            {
                ///insert or update UserToken add accesstoken for user
                var userTokenOld = _userTokenRepository.Find(c => c.UserId == userId && c.Name == deviceType);
                if (userTokenOld != null)
                {
                    userTokenOld.Value = accessToken;
                    userTokenOld.ExpiredTime = expiredTime;
                    _userTokenRepository.Update(userTokenOld);
                }
                else
                {
                    await _userTokenRepository.InsertAsync(new UserTokens
                    {
                        ExpiredTime = expiredTime,
                        LoginProvider = uuId,
                        Name = deviceType,
                        UserId = userId,
                        Value = accessToken
                    });
                }
            }
            else
            {
                await _userTokenRepository.InsertAsync(new UserTokens
                {
                    ExpiredTime = expiredTime,
                    LoginProvider = uuId,
                    Name = deviceType,
                    UserId = userId,
                    Value = accessToken
                });
            }
            await _unitOfWork.CompleteAsync();
        }
        public async Task<ResponseData> ExchangeRefreshToken(ExchangeRefreshTokenRequest message, string device, string RemoteIpAddress)
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (accessToken != null && accessToken.StartsWith("Bearer "))
            {
                accessToken = accessToken.Substring("Bearer ".Length).Trim();

                if (string.IsNullOrEmpty(device))
                {
                    return new ResponseData
                    {
                       //Err = ErrorTypeConstant.DeviceInvalid,
                        Content = StringMessage.ErrorMessages.DeviceInvalid,
                    };
                }
                var SigningKey = _authenticationSettings.SecretKey;
                var cp = GetPrincipalFromToken(accessToken, SigningKey);

                // invalid token/signing key was passed and we can't extract user claims
                if (cp != null)
                {
                    var userName = cp.Claims?.SingleOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                    var currentUser = await _userManager.Users.SingleOrDefaultAsync(r => r.UserName == userName);

                    var refreshTokenCurrent = GetRefreshTokens(userName);

                    var token = refreshTokenCurrent?.FirstOrDefault(x => x.Token == message.RefreshToken
                    && x.RemoteIpAddress == RemoteIpAddress);

                    if (token != null)
                    {
                        var newAccessToken = await LogInAsync(currentUser.Email);
                        var currentToken = _userTokenRepository.Find(c => c.UserId == currentUser.Id && c.Name == device && c.Value == accessToken);
                        if (currentToken != null)
                        {
                            currentToken.Value = accessToken;
                            _userTokenRepository.Update(currentToken);
                        }
                        var refreshToken = GenerateRefreshToken();

                        DeleteRefreshTokensFromUser(token);

                        await InsertRefreshTokens(currentUser.UserName, refreshToken, RemoteIpAddress);
                        var result = new ResponseData
                        {
                            Content = new
                            {
                                AccessToken = newAccessToken,
                                RefrreshToken = refreshToken
                            },
                        };
                        return result;
                    }
                }
            }          
            return null;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
        {
            try
            {
                return ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidateLifetime = true, // Check token expiration
                });
            }
            catch (SecurityTokenExpiredException ex)
            {
                // Handle token expired exception
                return null;
            }
            catch (Exception)
            {
                // Handle other exceptions
                return null;
            }
        }

        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IList<AspNetRefreshTokens> GetRefreshTokens(string username)
        {
            return _refreshTokensRepository.GetAll().Where(x => x.UserName == username).ToList();
        }

        public void DeleteRefreshTokensFromUser(AspNetRefreshTokens refreshToken)
        {
            _refreshTokensRepository.Delete(refreshToken);
        }


        public async Task DeleteRefreshTokens(string userName)
        {
            var token = _refreshTokensRepository.GetAll().FirstOrDefault(c => c.UserName == userName);
            if (token is null)
            {
                _logger.LogWarning("Token {refreshToken} không tồn tại trong database.");
                return;
            }
            _refreshTokensRepository.Delete(token);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRefreshTokensByUserName(string userName, string device)
        {
            var strategy = _csms_DbContext.Database.CreateExecutionStrategy();


            await strategy.ExecuteAsync(async () =>
            {
                await using var trans = _unitOfWork.BeginTransaction();
                try
                {
                    var refreshTokens = _refreshTokensRepository.GetAll().Where(r => r.UserName == userName).ToList();
                    if (!refreshTokens.Any())
                    {
                        _logger.LogWarning("Token {refreshToken} không tồn tại trong database.");
                        return;
                    }

                    var accessToken = _userTokenRepository.Find(c => c.UserId == userName && c.Name == device);
                    if (accessToken != null)
                    {
                        _userTokenRepository.Delete(accessToken);
                    }

                    _refreshTokensRepository.DeleteMulti(refreshTokens);
                    await _unitOfWork.CompleteAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Đã có lỗi xảy ra trong quá trình DeleteRefreshTokensByUserName");
                    await trans.RollbackAsync();
                    throw;
                }
            });

        }

        public async Task LogOut(CurrentUserDTO user, string refreshTokenPr, string remoteIpAddress)
        {
           
            var strategy = _csms_DbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var trans = _unitOfWork.BeginTransaction();
                try
                {
                    var refreshToken = _refreshTokensRepository.Find(c => 
                    c.UserName == user.UserName 
                    && c.Token == refreshTokenPr
                    && c.RemoteIpAddress == remoteIpAddress);

                    if (refreshToken is null) _logger.LogWarning($"Token {refreshTokenPr} không tồn tại trong database.");
                    else _refreshTokensRepository.Delete(refreshToken);

                    await _unitOfWork.CompleteAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Đã có lỗi xảy ra trong quá trình DeleteRefreshTokensByUserName");
                    await trans.RollbackAsync();
                }
            });
        }

        public async Task InsertUserLoginLog(UserApiLogRequest apiLog)
        {
            await _userLoginLog.InsertAsync(new UserLoginLog()
            {
                ApiName = apiLog.ApiName,
                UserId = apiLog.UserId,
                CreatedDate = DateTimeOffset.UtcNow,
                ModifiedBy = apiLog.UserName,
                ModifiedDate = DateTimeOffset.UtcNow,
                IpAddress = apiLog.IpAddress,
                CreatedBy = apiLog.UserName,
                FullName = apiLog.FullName,
            });
        }

        public async Task<IPagedList<UserApiLogRequest>> GetLogUserLogin(FilterLoginLog filter, string userId)
        {
            if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
            if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;
            var result = _userLoginLog.GetAll()
                        .Where(c => c.UserId == userId)
                        .Select(c => new UserApiLogRequest()
                        {
                            Id = c.Id,
                            ApiName = c.ApiName,
                            UserId = c.UserId,
                            FullName = c.FullName,
                            IpAddress = c.IpAddress,
                            UserName = c.CreatedBy,
                            CreateDate = c.CreatedDate
                        });

            if (!string.IsNullOrEmpty(filter.Sorting))
            {
                result = result.Sort(filter.Sorting);
            }
            else
            {
                result = result.OrderByDescending(x => x.CreateDate);
            }
            return result.ToPagedList(filter.PageIndex, filter.PageSize);
        }

    }
}
