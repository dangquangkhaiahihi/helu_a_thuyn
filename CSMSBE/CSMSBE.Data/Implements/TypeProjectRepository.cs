using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.District;
using CSMS.Model.DTO.Province;
using CSMSBE.Core.Extensions;
using CSMSBE.Infrastructure.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Commune = CSMS.Entity.CSMS_Entity.Commune;
using District = CSMS.Entity.CSMS_Entity.District;
using Province = CSMS.Entity.CSMS_Entity.Province;

namespace CSMS.Data.Implements
{
    public class TypeProjectRepository : ITypeProjectRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<TypeProjectRepository> _logger;
        private readonly IMapper _mapper;
        public TypeProjectRepository(csms_dbContext context, ILogger<TypeProjectRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IQueryable<TypeProject> GetLookupTypeProject(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<TypeProject> query = null;
                if (String.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.TypeProject.Where(x => x.IsDelete == false);
                    return query;
                }
                query = _context.TypeProject.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()) && x.IsDelete == false);
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
