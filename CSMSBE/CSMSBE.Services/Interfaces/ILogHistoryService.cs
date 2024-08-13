using CSMS.Entity.LogHistory;
using CSMS.Model.User;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface ILogHistoryService
    {
        bool Create(LogHistoryDTO logHistoryModel, CurrentUserDTO currentUserModel);
        IPagedList<LogHistoryViewDTO> GetAllLogHistory(int pageIndex, int pageSize, string sortExpression, int action, string userName, string description, DateTimeOffset? createDate);
        bool UpdateLogHistory(LogHistoryDTO logHistoryModel, CurrentUserDTO currentUserModel);
        bool DeleteLogHistoryById(int idLogHistory);
        LogHistoryEntity GetLogHistoryById(int id);
    }
}
