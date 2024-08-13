using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.LogHistory;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class LogHistoryRepository : BaseRepository<LogHistoryEntity>, ILogHistoryRepository
    {
        public LogHistoryRepository(csms_dbContext context) : base(context)
        {
        }
    }
}
