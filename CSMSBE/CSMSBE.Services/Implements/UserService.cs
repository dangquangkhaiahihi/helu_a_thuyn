using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Implements;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.IdentityExtensions;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.Province;
using CSMS.Model.DTO.UserDTO;
using CSMS.Model.IdentityAccess;
using CSMS.Model.User;
using CSMSBE.Core;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Core.Resource;
using CSMSBE.Core.Settings;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Services.Interfaces;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CSMSBE.Model.Repository;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace CSMSBE.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<UserLoginLog, int> _userLoginLog;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IAspNetRefreshTokensRepository _refreshTokensRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly ISecurityMatrixService _securityMatrixService;
        public UserService(IUserRepository userRepository,
            IHostingEnvironment env, 
            ILogger<UserService> logger, 
            IRepository<UserLoginLog, int> userLoginLog, 
            IUnitOfWork unitOfWork, 
            IUserTokenRepository userTokenRepository,
            IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, 
            IAspNetRefreshTokensRepository refreshTokensRepository,
            IMapper mapper,
            RoleManager<Role> roleManager,
            IRoleService roleService,
            ISecurityMatrixService securityMatrixService)
        {
            _userRepository = userRepository;
            _env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userLoginLog = userLoginLog;
            _unitOfWork = unitOfWork;
            _userTokenRepository = userTokenRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _roleService = roleService;
            _securityMatrixService = securityMatrixService;
        }

        public User GetByUserid(string id)
        {
            return _userRepository
                .Query(x => x.Id == id).Include(x => x.UserRoles)
                .FirstOrDefault();
        }

        public string GetRoleByUserId(string id)
        {
            return _userRepository
                .Query(x => x.Id == id)
                .Include(x => x.UserRoles)
                .ThenInclude(e => e.Role).Select(x => x.UserRoles.Select(e => e.Role.Code).FirstOrDefault())
                .FirstOrDefault();
        }

        public string GetUserClaimByUserId(string id)
        {
            var userClaim = _userRepository
                .Query(x => x.Id == id)
                .Include(x => x.UserClaims)
                .Select(x => x.UserClaims.FirstOrDefault(c => c.ClaimType == Constant.RecordPermission))
                .FirstOrDefault();
            if (userClaim == null)
            {
                return string.Empty;
            }
            return userClaim.ClaimValue;
        }


        public DetailUserDTO GetUserDetail(string id)
        {
            var data = _userRepository
                .Query(x => x.Id == id)
                .Include(x => x.UserRoles).ThenInclude(e => e.Role);

            ///get the last time user login in system
           var loginLatest = _userLoginLog.GetAll().Where(c => c.UserId == id)
                                                    .OrderByDescending(c => c.CreatedDate)
                                                    .FirstOrDefault()?.CreatedDate;
            ///get list online, offline for user
            var deviceStatus = _userTokenRepository.GetAll().Where(c => c.UserId == id && c.ExpiredTime.HasValue)
                                                 .Select(c => new { c.UserId, c.Name, c.ExpiredTime }).ToList()
                                                 .GroupBy(c => c.UserId)
                                                 .Select(g => new { Status = string.Join(", ", g.Select(i => $"{i.Name}: {i.ExpiredTime.Value.Subtract(DateTimeOffset.Now).TotalSeconds > 0}")) }).FirstOrDefault();

            // Get screen data for the role
            var userRoles = data.Select(x => x.UserRoles.FirstOrDefault()).FirstOrDefault();
            var roleId = userRoles?.Role.Id;
            var roleName = userRoles?.Role.Code;
            var screenData = _securityMatrixService.GetDetailSecurityMatrix(roleId);

            var result = data.ToList().Select(x =>
            new DetailUserDTO
            {
                Id = x.Id,             
                Email = x.Email,
                ModifiedBy = x.ModifiedBy,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                FullName = x.FullName,
                Status = x.Status,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                Address = x.Address,
                ModifiedDate = x.ModifiedDate,
                PhoneNumber = x.PhoneNumber,
                UserType = x.UserType,
                RoleId = roleId,
                RoleName = roleName,
                AvatarUrl = x.AvatarUrl
            }).FirstOrDefault();

            if (loginLatest.HasValue)
            {
                result.LastDateLogin = loginLatest.Value.ToString();
            }
            if (deviceStatus != null)
            {
                result.DeviceStatus = deviceStatus.Status;
            }
            return result;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        public string GetCurrentUserName()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return null;
            }
            else
            {
                var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                return user?.UserName;
            }
        }
        public async Task<bool> KickOutUser(string userId)
        {
            using var trans = _unitOfWork.BeginTransaction();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Không tìm thấy user này trong system", userId);
                    return false;
                }
                var userRefreshToken = _refreshTokensRepository.FindAll(c => c.UserName == user.UserName);
                var userAccessToken = _userTokenRepository.FindAll(c => c.UserId == userId);
                if (userRefreshToken.Any())
                {
                    _refreshTokensRepository.DeleteMulti(userRefreshToken);
                }
                if (userAccessToken.Any())
                {
                    _userTokenRepository.DeleteMulti(userAccessToken);
                }
                await _unitOfWork.CompleteAsync();
                await trans.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Đã có lỗi xảy ra trong quá trình xóa refresh và access token", ex);
                await trans.RollbackAsync();
                return false;
            }
        }



        public async Task<bool> UserLogin(User user, ExternalLoginInfor externalLoginInfor, UserLoginInfo userInfo)
        {

            var newUser = await _userManager.CreateAsync(user);
            if (!newUser.Succeeded)
            {
                ///throw login exception
                _logger.LogInformation($"{StringMessage.ErrorMessages.CannotCreateAccount}: {externalLoginInfor.ExternalLoginId}");
                return false;
            }
            _ = await _userManager.AddToRoleAsync(user, RoleHelper.RegisterUser);

            ///innit userid to authenication membership
            await _userManager.AddLoginAsync(user, userInfo);

            ///add claim
            var claims = new List<Claim>();
            claims.Add(new Claim(Constant.ExternalLogin.NameIdentifier, externalLoginInfor.ExternalLoginId));
            claims.Add(new Claim(Constant.ExternalLogin.FirstName, externalLoginInfor.FirstName));
            claims.Add(new Claim(Constant.ExternalLogin.LastName, externalLoginInfor.LastName));

            await _userManager.AddClaimsAsync(user, claims);
            return true;
        }


        private static List<FilterExtensions.FilterParams> BuildParams(string email)
        {
            var filterParams = new List<FilterExtensions.FilterParams>();
            if (!string.IsNullOrEmpty(email))
                filterParams.Add(new FilterExtensions.FilterParams
                {
                    ColumnName = "Email",
                    FilterValue = email,
                    FilterOption = FilterExtensions.FilterOptions.Contains
                });
            return filterParams;
        }

        public async Task<IList<UserLookup>> GetLookUpUsers(IKeywordDto keywordDto)
        {
            try
            {
                var users = await _userRepository.GetLookupUser(keywordDto).ToListAsync();
                if (users != null)
                {
                    var userLookup = await Task.Run(() => users.AsParallel().Select(dto => _mapper.Map<UserLookup>(dto)).ToList());
                    return userLookup;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IQueryable<User>> CreateQuery(UserFilterRequest filter)
        {

            var query = _userRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.FullName), x => x.FullName.ToLower().Contains(filter.FullName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.PhoneNumber), x => x.PhoneNumber.Contains(filter.PhoneNumber))
                .WhereIf(!string.IsNullOrEmpty(filter.Email), x => x.Email.ToLower().Contains(filter.Email.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.UserType), x => x.UserType == filter.UserType);

            return await Task.FromResult(query);
        }

        private IQueryable<User> ApplySortingToQuery(IQueryable<User> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(User), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<User>(resultExpression);
            }

            return query;
        }
        public async Task<IPagedList<UserDTO>> FilterUsers(UserFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();
                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);


                var resultItems = await pagedQuery
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                var pagedList = new PagedList<UserDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public UserDTO UpdateUser(UpdateUserDTO model)
        {
            try
            {
                var update = _userRepository.UpdateUser(model);

                if (update == null)
                {
                    throw new InvalidOperationException("Update operation failed");
                }

                var userDto = _mapper.Map<UserDTO>(update);

                // Get user roles
                var userRoles = _userRepository.GetListUserRole(update.Id);

                if (userRoles.Any())
                {
                    var role = _userRepository.GetRoleDetail(update.Id);
                    userDto.RoleId = role?.Id;
                    userDto.RoleName = role?.Code;
                }
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            var role = _roleService.GetById(roleId);
            if (role == null)
            {
                throw new InvalidOperationException("Role not found");
            }
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            return result;
        }

        public async Task<bool> UploadAvatar(UploadAvatarDTO dto)
        {
            var userId = dto.UserId;
            if (string.IsNullOrEmpty(userId)) return false;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = dto.Avatar.FileName;
            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser != null && !string.IsNullOrEmpty(existingUser.AvatarUrl))
            {
                var existingFilePath = Path.Combine(folderPath, Path.GetFileName(existingUser.AvatarUrl));
                if (File.Exists(existingFilePath))
                {
                    File.Delete(existingFilePath);
                }
            }
            await FileHelper.SaveFileAsync(dto.Avatar, folderPath);
            var urlPath = $"uploads/avatars/{fileName}";
            var result = await _userRepository.UploadAvatar(urlPath, userId);
            return result;
        }
    }
}
