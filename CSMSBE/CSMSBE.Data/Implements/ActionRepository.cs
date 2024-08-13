using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class ActionRepository : BaseRepository<Entity.SecurityMatrix.Action>, IActionRepository
    {
        private readonly csms_dbContext _context;
        public ActionRepository(csms_dbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Entity.SecurityMatrix.Action> GetLookUpAction(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<Entity.SecurityMatrix.Action> query = null;
                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Actions;
                    return query;
                }
                query = _context.Actions.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
