using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Entity.LogHistory;
using CSMS.Model.User;
using CSMSBE.Core.Extensions;
using CSMSBE.Core.Interfaces;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Core.Enum;
using CSMSBE.Model.Repository;

namespace CSMSBE.Services.Implements
{
    public class LogHistoryService : ILogHistoryService
    {
        private readonly ILogHistoryRepository _logHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LogHistoryService> _logger;

        public LogHistoryService(ILogHistoryRepository logHistoryRepository, IMapper mapper, IUnitOfWork unitOfWork, ILogger<LogHistoryService> logger)
        {
            _logHistoryRepository = logHistoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public bool Create(LogHistoryDTO logHistoryModel, CurrentUserDTO currentUserModel)
        {
            using var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                LogHistoryEntity logHistoryEntity = _mapper.Map<LogHistoryEntity>(logHistoryModel);
                logHistoryEntity.CreatedDate = DateTimeOffset.Now;
                logHistoryEntity.ModifiedDate = DateTimeOffset.Now;
                logHistoryEntity.CreatedBy = currentUserModel.UserName;
                logHistoryEntity.ModifiedBy = currentUserModel.UserName;
                _logHistoryRepository.Insert(logHistoryEntity);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public IPagedList<LogHistoryViewDTO> GetAllLogHistory(int pageIndex, int pageSize, string sortExpression, int action,
            string userName, string description, DateTimeOffset? createDate)
        {
            try
            {
                var logHistories = _logHistoryRepository.Query(x => x.Id > 0).AsQueryable();
                if (action != 0)
                {
                    logHistories = logHistories.Where(x => x.Action == action);
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    logHistories = logHistories.Where(x => x.UserName.ToLower().Contains(userName.ToLower()));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    logHistories = logHistories.Where(x => x.Description.ToLower().Contains(description.ToLower()));
                }

                if (!createDate.Equals(null))
                {
                    logHistories = logHistories.Where(x => x.CreatedDate >= createDate && x.CreatedDate <= (createDate.Value.AddDays(1).Date));
                }

                return (IPagedList<LogHistoryViewDTO>)logHistories.Select(x => new LogHistoryViewDTO()
                {
                    Id = x.Id,
                    Action = x.Action,
                    ActionName = GetActionName(x.Action),
                    UserName = x.UserName,
                    Description = x.Description,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate
                }).Sort(sortExpression).ToPagedList(pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return null;
            }
        }

        public bool UpdateLogHistory(LogHistoryDTO logHistoryModel, CurrentUserDTO currentUserModel)
        {
            using var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                LogHistoryEntity logHistoryEntity = GetLogHistoryById(logHistoryModel.Id);
                if (logHistoryEntity == null)
                {
                    return false;
                }
                logHistoryEntity.UserName = logHistoryModel.UserName;
                logHistoryEntity.Action = logHistoryModel.Action;
                logHistoryEntity.Description = logHistoryModel.Description;
                logHistoryEntity.ModifiedDate = DateTimeOffset.UtcNow;
                logHistoryEntity.ModifiedBy = currentUserModel.UserName;
                _logHistoryRepository.Update(logHistoryEntity);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public bool DeleteLogHistoryById(int idLogHistory)
        {
            using var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                LogHistoryEntity logHistoryEntity = GetLogHistoryById(idLogHistory);
                if (logHistoryEntity == null)
                {
                    return false;
                }
                bool isDelete = _logHistoryRepository.Delete(logHistoryEntity);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return isDelete;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public LogHistoryEntity GetLogHistoryById(int id)
        {
            return _logHistoryRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public static string GetActionName(int id)
        {
            foreach (var value in Enum.GetValues(typeof(ActionEnum)))
            {
                if (value.GetHashCode() == id) return value.ToString();
            }
            return "";
        }
    }
}