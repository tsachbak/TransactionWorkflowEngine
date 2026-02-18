using TransactionWorkflowEngine.Dtos;

namespace TransactionWorkflowEngine.Services.HistoryService
{
    public interface IHistoryService
    {
        /// <summary>
        /// Saves a new transaction status change history record to the database.
        /// </summary>
        Task AddAsync(Guid transactionId, int fromStatusId, int toStatusId, string? reason, CancellationToken ct);

        /// <summary>
        /// Get the history of status changes for a specific transaction.
        /// </summary>
        Task<List<TransactionHistoryItemDto>> GetHistoryByTransactionIdAsync(Guid transactionId, CancellationToken ct);
    }
}
