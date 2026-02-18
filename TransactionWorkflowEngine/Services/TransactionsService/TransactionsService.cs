using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.TransactionsService
{
    public class TransactionsService : ITransactionsService
    {
        private readonly AppDbContext _db;
        public TransactionsService(AppDbContext db)
        {
            _db = db;
        }

        public Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct)
        {
            return _db.Transactions
                .Include(t => t.CurrentStatus)
                .FirstOrDefaultAsync(t => t.Id == transactionId, ct);
        }

        public async Task<Transaction> CreateTransactionAsync(int initialStatusId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                CurrentStatusId = initialStatusId,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync(ct);

            await _db.Entry(transaction)
                .Reference(t => t.CurrentStatus)
                .LoadAsync(ct);

            return transaction;
        }
    }
}
