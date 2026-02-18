using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private readonly AppDbContext _db;

        public HistoryService(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Guid transactionId, int fromStatusId, int toStatusId, string? reason, CancellationToken ct)
        {
            var historyRecord = new TransactionStatusHistory
            {
                TransactionId = transactionId,
                FromStatusId = fromStatusId,
                ToStatusId = toStatusId,
                ChangedAt = DateTime.UtcNow,
                Reason = reason
            };

            _db.TransactionStatusHistories.Add(historyRecord);
            await _db.SaveChangesAsync(ct);
        }
    }
}
