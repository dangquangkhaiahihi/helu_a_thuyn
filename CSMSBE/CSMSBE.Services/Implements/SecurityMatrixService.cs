using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMS.Model.SecurityMatrix;
using CSMS.Model.User;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Implements;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Model.Repository;
using CSMSBE.Services.Interfaces;
using ImageMagick;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class SecurityMatrixService : ISecurityMatrixService
    {
        private readonly ISecurityMatrixRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SecurityMatrixService> _logger;
        private readonly IActionRepository _actionRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly csms_dbContext _context;
        public SecurityMatrixService(ILogger<SecurityMatrixService> logger, IUnitOfWork unitOfWork, ISecurityMatrixRepository repository, IActionRepository actionRepository, IScreenRepository screenRepository,
            UserManager<User> userManager, IMapper mapper, csms_dbContext context)
        {
            _repository = repository;
            _actionRepository = actionRepository;
            _screenRepository = screenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ICollection<ScreenDTO>> GetListScreen(string userId)
        {
            var user = _userManager.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == userId).Select(e => new CurrentUserDTO
                {
                    Id = e.Id,
                    UserName = e.UserName,
                    Email = e.Email,
                    FullName = e.FullName,
                    UserType = e.UserType,
                    Roles = e.UserRoles.Select(c => new RoleDTO
                    {
                        Id = c.Role.Id,
                        Code = c.Role.Code
                    }).ToList()
                }).FirstOrDefault();

            if (user == null) return null;

            var roleAdmin = user.Roles.Any(x => x.Code == RoleHelper.Admin);

            var roles = user.Roles.Select(x => x.Id);
            var query = _repository.GetAll().Include(x => x.Screen.Parent)
                .WhereIf(!roleAdmin, x => roles.Contains(x.RoleId))
                .Select(x => x.Screen)
                .Distinct()
                .ToList().OrderBy(x => x.Order);
            var response = _mapper.Map<ICollection<ScreenDTO>>(query);
            var parent = response.Where(x => x.Parent != null).Select(x => x.Parent).Distinct().Union(response.Where(x => x.Parent == null && x.Code != null)).OrderBy(x => x.Order);
            foreach (var item in parent)
            {
                var childrent = response.Where(x => x.ParentId == item.Id).ToList();
                item.Childrent = childrent;
            }

            return parent.ToList();
        }

       

        public List<ScreenViewDTO> GetDetailSecurityMatrix(string RoleId)
        {
            var listScreen = _repository.Query(x => x.RoleId == RoleId).Include(x => x.Screen).Select(x => new ScreenViewDTO
            {
                ScreenId = x.ScreenId,
                ScreenName = x.Screen.Name
            });
            var screen = listScreen.Distinct().ToList();
            foreach (var entry in screen)
            {
                var listAction = _repository.Query(x => x.ScreenId == entry.ScreenId && x.RoleId == RoleId).Select(x => new ActionViewDTO
                {
                    ActionId = x.ActionId,
                    ActionName = x.Action.Name
                }).ToList();
                entry.Actions = listAction;
            }
            return screen;
        }
        public async Task<bool> CreateSecurityMatrix(CreateSecurityMatrixDTO model)
        {
            try
            {
                var deleted = _repository.Query(x => x.RoleId == model.RoleId).ToList();
                _repository.DeleteMulti(deleted);

                if (!model.Screens.Any())
                {
                    return true;
                }

                foreach (var screen in model.Screens)
                {
                    foreach (var action in screen.Actions.Distinct())
                    {
                        var entry = new SecurityMatrices
                        {
                            RoleId = model.RoleId,
                            ScreenId = screen.ScreenId,
                            ActionId = action.ActionId
                        };
                        _repository.Insert(entry);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }
        public async Task<bool> CheckPermission(string roleName, string actionName, string screenName)
        {
            return await _repository.CheckPermission(roleName, actionName, screenName);
        }

        #region Private

        private List<FilterExtensions.FilterParams> MapParams(string roleName, string screenName)
        {
            var filterParams = new List<FilterExtensions.FilterParams>();
            if (!string.IsNullOrEmpty(roleName))
                filterParams.Add(new FilterExtensions.FilterParams()
                {
                    ColumnName = "RoleName",
                    FilterValue = roleName
                });
            if (!string.IsNullOrEmpty(screenName))
                filterParams.Add(new FilterExtensions.FilterParams()
                {
                    ColumnName = "ScreenName",
                    FilterValue = screenName
                });
            return filterParams;
        }

        public async Task<IList<LookupDTO>> GetLookUpScreen(IKeywordDto keywordDto)
        {
            try
            {
                var screens = await _screenRepository.GetLookUpScreen(keywordDto).ToListAsync();
                if (screens != null)
                {
                    var lookupScreensDto = await Task.Run(() => screens.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupScreensDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<LookupDTO>> GetLookUpAction(IKeywordDto keywordDto)
        {
            try
            {
                var actions = await _actionRepository.GetLookUpAction(keywordDto).ToListAsync();
                if (actions != null)
                {
                    var lookupActionsDto = await Task.Run(() => actions.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupActionsDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<SecurityMatrixDTO>> FilterSM(SMFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
                var resultItems = await pagedQuery.Select(e => new SecurityMatrixDTO
                {
                    Id = e.Id,
                    RoleName = e.Role.Name,
                    RoleId = e.Role.Id,
                    ScreenId = e.Screen.Id,
                    ScreenName = e.Screen.Name,
                    Actions = _repository.GetAll()
                    .Include(securityMatrix => securityMatrix.Action)
                    .Where(s => s.Id == e.Id).Select(e => new ActionLookupDTO
                    {
                        Id = e.Action.Id,
                        Name = e.Action.Name
                    }).ToList()

                }).ToListAsync();

                var pagedList = new PagedList<SecurityMatrixDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IQueryable<SecurityMatrices>> CreateQuery(SMFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }

            var query = _repository.GetAll()
                .Include(e => e.Screen)
                .Include(e => e.Action)
                .Include(e => e.Role)
                .WhereIf(!string.IsNullOrEmpty(filter.ScreenName), x => x.Screen.Name.ToLower().Contains(filter.ScreenName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.RoleName), x => x.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));


            return await Task.FromResult(query);
        }

        private IQueryable<SecurityMatrices> ApplySortingToQuery(IQueryable<SecurityMatrices> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(SecurityMatrices), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<SecurityMatrices>(resultExpression);
            }

            return query;
        }

        public async Task<bool> HasPermissionAsync(string roleId, string screen, string action)
        {
            return await _repository.CheckPermission(roleId, action, screen);
        }

        #endregion
    }
}
