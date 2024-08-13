using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSMSBE.Services.Interfaces;
using CSMSBE.Services.BaseServices.Interfaces;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Core.Interfaces;
using CSMSBE.Core.Helper;
using CSMS.Model.DTO.BaseFilterRequest.BaseModels;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Data.Repository;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Model.Repository;
using CSMSBE.Core.Extensions;

namespace CSMSBE.Services.BaseServices.Implements
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TEntityDto">DTO tương ứng với Entity</typeparam>
    /// <typeparam name="TListEntityDto">DTO dùng cho các trường hợp list, không có next object</typeparam>
    /// <typeparam name="TCreateDto">DTO Create</typeparam>
    /// <typeparam name="TUpdateDto">DTO Update</typeparam>
    /// <typeparam name="TFilterDto">DTO filter dữ liệu</typeparam>
    public class BaseService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto, TFilterDto> :
        BaseNoFilterService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>,
        IBaseService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto, TFilterDto>
        where TEntityDto : EntityDto<T>
        where TListEntityDto : EntityDto<T>
                                      where TCreateDto : class
                                      where TUpdateDto : EntityDto<T>
                                      where TEntity : BaseFullAuditedEntity<T>
        where TFilterDto : PagedResultRequestDto
    {
        public BaseService(IRepository<TEntity, T> repository, IMapper mapper,
            ILogger<BaseService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto, TFilterDto>> logger,
            IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
            //,IAppMemoryCache<T, TEntity> memoryCache, bool useCache=false)
            : base(repository, mapper, logger, unitOfWork, httpContextAccessor, userManager)
        {
        }

        public async virtual Task<IQueryable<TEntity>> QueryFilter(TFilterDto filter)
        {
            IQueryable<TEntity> result;
            //if (_useCache)
            //    result = (await _memoryCache.GetAll()).AsQueryable();
            //else
            result = _repository.GetAll().OrderByDescending(x => x.ModifiedDate);

            return result;
        }

        #region Get
        public async virtual Task<ICollection<TListEntityDto>> GetAllNoPage(TFilterDto filter)
        {
            try
            {
                var query = (await QueryFilter(filter));
                var response = _mapper.Map<ICollection<TListEntityDto>>(query.ToList());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async virtual Task<IPagedList<TListEntityDto>> GetAll(TFilterDto filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;
                var query = (await QueryFilter(filter));
                var response = query.ToPagedList(filter.PageIndex, filter.PageSize);
                var result = response.Map<TListEntityDto, TEntity>(_mapper);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        #endregion
    }
}
