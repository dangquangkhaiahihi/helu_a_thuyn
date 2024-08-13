using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class FileExtensionsRepository : IFileExtensionsRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<FileExtensionsRepository> _logger;
        private readonly IMapper _mapper;

        public FileExtensionsRepository(csms_dbContext context, ILogger<FileExtensionsRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<FileExtensions> GetByName(string name)
        {
            try
            {
                return await _context.FileExtensions.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FileExtensions>> GetAll()
        {
            try
            {
                return await _context.FileExtensions.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<FileExtensions> GetLookupFileExtension(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<FileExtensions> query = null;
                if (String.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.FileExtensions;
                    return query;
                }
                query = _context.FileExtensions.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
