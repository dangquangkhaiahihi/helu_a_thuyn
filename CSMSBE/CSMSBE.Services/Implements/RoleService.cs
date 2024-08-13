using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity.IdentityAccess;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Role;
using CSMS.Model.User;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Implements;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Infrastructure.Implements;

using CSMS.Entity.CSMS_Entity;
using System.Linq.Expressions;
using CSMSBE.Core.Helper;
using CSMSBE.Model.Repository;

namespace CSMSBE.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleService> _logger;
        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public RoleViewDTO GetDetail(string id)
        {
            return _roleRepository.Query(x => x.Id == id).Select(x => _mapper.Map<RoleViewDTO>(x)).FirstOrDefault();
        }

        public string GetRoleNameById(string id)
        {
            var data = _roleRepository.Find(x => x.Id == id);
            return data != null ? data.Name : null;
        }

        public Role GetById(string id)
        {
            return _roleRepository.Query(x => x.Id == id)
                .FirstOrDefault();
        }

        public bool GetByCode(string code)
        {
            return _roleRepository.Query(x => x.Code == code)
                       .FirstOrDefault() != null;
        }
      

        public bool RemoveRole(string id)
        {
            try
            {
                var result = _roleRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public bool CheckRoleById(string id, string roleCode)
        {
            return _roleRepository.Find(x => x.Id == id)?.Code == roleCode;
        }


        public async Task<IList<RoleDTO>> GetLookupRole(IKeywordDto keywordDto)
        {
            try
            {
                var roles = await _roleRepository.GetLookupRole(keywordDto).ToListAsync();
                if (roles != null)
                {
                    var rolesDto = _mapper.Map<List<RoleDTO>>(roles);
                    return rolesDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IPagedList<RoleDTO>> FilterRole(RoleFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);

                var resultItems = await pagedQuery.ProjectTo<RoleDTO>(_mapper.ConfigurationProvider).ToListAsync();

                var pagedList = new PagedList<RoleDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IQueryable<Role>> CreateQuery(RoleFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }

            var query = _roleRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Code), x => x.Code.ToLower().Contains(filter.Code.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()));
                
            return await Task.FromResult(query);
        }

        private IQueryable<Role> ApplySortingToQuery(IQueryable<Role> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(Role), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<Role>(resultExpression);
            }

            return query;
        }

        public RoleDTO GetRoleById(string id)
        {
            try
            {
                var result = _mapper.Map<RoleDTO>(_roleRepository.Get(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public RoleDTO CreateRole(CreateRoleDTO dto, string userId)
        {
            try
            {

                var result = _roleRepository.Create(dto, userId);
                return _mapper.Map<RoleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public RoleDTO UpdateRole(UpdateRoleDTO dto, string userId)
        {
            try
            {
                var result = _roleRepository.Update(dto, userId);
                return _mapper.Map<RoleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

    }
}
