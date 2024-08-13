using CSMS.Data.Repository;
using CSMS.Entity.LogHistory;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface ILogHistoryRepository : IBaseRepository<LogHistoryEntity>
    {
    }
}
