using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Implements;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Services.BaseServices;
using CSMSBE.Services.Interfaces;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Expressions;

using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.SomeTableDTO;
using CSMS.Model.DTO.FilterRequest;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSMSBE.Services.Implements
{
    public class SomeTableService : ISomeTableService
    {
        private readonly ISomeTableRepository _someTableRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SomeTableService> _logger;
        public SomeTableService(ISomeTableRepository someTableRepository,
            IMapper mapper, ILogger<SomeTableService> logger)
        {
            _someTableRepository = someTableRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public SomeTableDTO GetSomeTableById(int id)
        {
            try
            {
                var result = _mapper.Map<SomeTableDTO>(_someTableRepository.Get(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IQueryable<SomeTableDTO> GetLookupSomeTable()
        {
            try
            {
                var result = _someTableRepository.GetAll().ProjectTo<SomeTableDTO>(_mapper.ConfigurationProvider);
                return result;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IPagedList<SomeTableDTO>> FilterSomeTable(SomeTableFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();
                var pagedQuery = query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
                var sortedPagedQuery = ApplySortingToQuery(pagedQuery, filter.Sorting);

                var resultItems = await sortedPagedQuery.ProjectTo<SomeTableDTO>(_mapper.ConfigurationProvider).ToListAsync();

                var pagedList = new PagedList<SomeTableDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IQueryable<SomeTable>> CreateQuery(SomeTableFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }

            var query = _someTableRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.NormalText), x => x.NormalText.ToLower().Contains(filter.NormalText.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.PhoneNumber), x => x.PhoneNumber.Contains(filter.PhoneNumber))
                .WhereIf(!string.IsNullOrEmpty(filter.Email), x => x.Email.ToLower().Contains(filter.Email.ToLower()))
                .WhereIf(filter.StartDate.HasValue, x => x.CreatedDate >= filter.StartDate)
                .WhereIf(filter.EndDate.HasValue, x => x.CreatedDate <= filter.EndDate)
                .WhereIf(filter.Status.HasValue, x => x.Status == filter.Status)
                .WhereIf(!string.IsNullOrEmpty(filter.Type), x => x.Type == filter.Type)
                .Where(x => x.IsDelete == filter.IsDelete);

            return await Task.FromResult(query);
        }

        private IQueryable<SomeTable> ApplySortingToQuery(IQueryable<SomeTable> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(SomeTable), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<SomeTable>(resultExpression);
            }

            return query;
        }


        public bool RemoveSomeTable(int id)
        {
            try
            {
                var result = _someTableRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        
        public SomeTableDTO CreateSomeTable(CreateSomeTableDTO dto)
        {
            try
            {
                var someTableEntity = _mapper.Map<SomeTable>(dto);
                var result = _someTableRepository.Create(someTableEntity);
                return _mapper.Map<SomeTableDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public SomeTableDTO UpdateSomeTable(UpdateSomeTableDTO dto)
        {
            try
            {
                var someTableEntity = _mapper.Map<SomeTable>(dto);
                var result = _someTableRepository.Update(someTableEntity);
                return _mapper.Map<SomeTableDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
