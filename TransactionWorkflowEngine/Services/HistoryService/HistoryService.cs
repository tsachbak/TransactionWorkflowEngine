using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Dtos;
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

        public async Task<List<TransactionHistoryItemDto>> GetHistoryByTransactionIdAsync(Guid transactionId, CancellationToken ct)
        {
            var historyItems = await _db.TransactionStatusHistories
                .AsNoTracking()
                .Where(h => h.TransactionId == transactionId)
                .Join(
                    _db.TransactionStatuses.AsNoTracking(),
                    h => h.FromStatusId, 
                    fromStatus => fromStatus.Id, 
                    (h, fromStatus) => new { History = h, FromStatus = fromStatus }
                )
                .Join(
                    _db.TransactionStatuses
                    .AsNoTracking(),
                    x => x.History.ToStatusId,
                    toStatus => toStatus.Id,
                    (x, toStatus) => new TransactionHistoryItemDto
                    {
                        FromStatusId = x.History.FromStatusId,
                        FromStatusName = x.FromStatus.Name,
                        ToStatusId = x.History.ToStatusId,
                        ToStatusName = toStatus.Name,
                        ChangedAt = x.History.ChangedAt,
                        Reason = x.History.Reason
                    }
                )
                .OrderBy(i => i.ChangedAt)
                .ToListAsync(ct);

            return historyItems;
        }
    }
}
