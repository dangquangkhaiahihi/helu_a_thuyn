using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity.Issues;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMS.Model.Issue;
using CSMS.Model.SecurityMatrix;
using CSMS.Model.User;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Services.Interfaces;
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
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IssueService> _logger;
        public IssueService(IIssueRepository issueRepository, IMapper mapper, ILogger<IssueService> logger) 
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IPagedList<IssueDTO>> FilterIssue(IssueFilterRequest filter)
        {
            try
            {
                if (filter.PageIndex == 0) filter.PageIndex = Constant.DefaultPageIndex;
                if (filter.PageSize == 0) filter.PageSize = Constant.DefaultPageSize;

                var query = await CreateQuery(filter);

                var totalItemCount = await query.CountAsync();

                var sortedPagedQuery = ApplySortingToQuery(query, filter.Sorting);
                var pagedQuery = sortedPagedQuery.Include(i => i.Comments) 
                    .ThenInclude(c => c.User) .Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
                var resultItems = await pagedQuery
                        .Select(i => new IssueDTO
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Type = i.Type,
                            Status = i.Status,
                            Description = i.Description,
                            UserId = i.UserId,
                            UserName = i.User.UserName,
                            ProjectId = i.ProjectId,
                            ProjectName = i.Project.Name,
                            CreatedDate = i.CreatedDate,
                            CreatedBy = i.CreatedBy,
                            ModifiedDate = i.ModifiedDate,
                            ModifiedBy = i.ModifiedBy,         
                            IsDelete = i.IsDelete,
                        })
                        .Where(i => i.IsDelete == false)
                        .ToListAsync();

                var pagedList = new PagedList<IssueDTO>(resultItems, filter.PageIndex, filter.PageSize, totalItemCount);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IQueryable<Issue>> CreateQuery(IssueFilterRequest filter)
        {
            if (!filter.IsDelete.HasValue)
            {
                filter.IsDelete = false;
            }
            var query = _issueRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.ToLower().Contains(filter.Name.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Type), x => x.Type.ToLower().Contains(filter.Type.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(filter.Status), x => x.Status.ToLower().Contains(filter.Status.ToLower()));


            return await Task.FromResult(query);
        }

        private IQueryable<Issue> ApplySortingToQuery(IQueryable<Issue> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var sortParams = sorting.Split(' ');
                var sortBy = sortParams[0];
                var sortOrder = sortParams.Length > 1 ? sortParams[1] : "asc";

                var parameter = Expression.Parameter(typeof(Issue), "x");
                var property = Expression.Property(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = sortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type },
                    query.Expression, Expression.Quote(lambda));
                query = query.Provider.CreateQuery<Issue>(resultExpression);
            }

            return query;
        }
    
        public IssueDTO GetIssueById(int id)
        {

            try
            {
                var result = _mapper.Map<IssueDTO>(_issueRepository.Get(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IssueDTO CreateIssue(CreateIssueDTO dto, string userId)
        {
            try
            {

                var result = _issueRepository.Create(dto, userId);
                return _mapper.Map<IssueDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IssueDTO UpdateIssue(UpdateIssueDTO dto, string userId)
        {
            try
            {
                var updatedEntity = _issueRepository.Update(dto, userId);
                var issueDto = _mapper.Map<IssueDTO>(updatedEntity);
                issueDto.ProjectName = updatedEntity.Project.Name;
                return issueDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public bool RemoveIssue(int id)
        {
            try
            {
                var result = _issueRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
