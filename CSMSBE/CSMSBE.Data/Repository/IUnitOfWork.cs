using System.Threading.Tasks;
using CSMS.Entity;

using Microsoft.EntityFrameworkCore.Storage;

namespace CSMSBE.Model.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync();
        bool Complete();
        Task RecoverIncompleteTransactions();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly csms_dbContext _context;

        public UnitOfWork(csms_dbContext context)
        {
            _context = context;
        }

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Complete()
        {
            return _context.SaveChanges() > 0;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task RecoverIncompleteTransactions()
        {
            var incompleteTransactions = _context.TransactionLogs
                .Where(log => log.status == "Started" || log.status == "Failed")
                .ToList();

            foreach (var transaction in incompleteTransactions)
            {
                if (transaction.TransactionType == "UpdateModel")
                {
                    // Thực hiện lại hoặc khôi phục update model
                }
            }
        }
    }
}
