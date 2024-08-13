using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class ScreenRepository : BaseRepository<Screen>, IScreenRepository
    {
        private readonly csms_dbContext _context;
        public ScreenRepository(csms_dbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Screen> GetLookUpScreen(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<Screen> query = null;
                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Screens;
                    return query;
                }
                query = _context.Screens.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

