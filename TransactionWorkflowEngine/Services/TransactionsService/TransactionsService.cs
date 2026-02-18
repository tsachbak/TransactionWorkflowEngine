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

        public async Task UpdateStatusAsync(Guid transactionId, int newStatusId, CancellationToken ct)
        {
            var transaction = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId, ct);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID '{transactionId}' not found.");

            transaction.CurrentStatusId = newStatusId;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateStatusWithHistoryAsync(Transaction transaction, int toStatusId, string? reason, CancellationToken ct)
        {
            var fromStatusId = transaction.CurrentStatusId;

            // Update current state on the tracked entity.
            transaction.CurrentStatusId = toStatusId;
            transaction.UpdatedAt = DateTime.UtcNow;

            // Add the corresponding audit trail entry in the same DbContext unit of work.
            _db.TransactionStatusHistories.Add(new TransactionStatusHistory
            {
                TransactionId = transaction.Id,
                FromStatusId = fromStatusId,
                ToStatusId = toStatusId,
                ChangedAt = DateTime.UtcNow,
                Reason = reason
            });

            // A single SaveChanges call keeps status and history atomic.
            await _db.SaveChangesAsync(ct);
        }
    }
}
