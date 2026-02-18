using TransactionWorkflowEngine.Dtos;

namespace TransactionWorkflowEngine.Services.HistoryService
{
    /// <summary>
    /// Manages status transition history persistence and retrieval.
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Saves a new transaction status change history record.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="fromStatusId">Source status identifier.</param>
        /// <param name="toStatusId">Target status identifier.</param>
        /// <param name="reason">Optional business reason for the transition.</param>
        /// <param name="ct">Cancellation token.</param>
        Task AddAsync(Guid transactionId, int fromStatusId, int toStatusId, string? reason, CancellationToken ct);

        /// <summary>
        /// Gets the status-change history for a transaction.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Ordered history items for the transaction.</returns>
        Task<List<TransactionHistoryItemDto>> GetHistoryByTransactionIdAsync(Guid transactionId, CancellationToken ct);
    }
}
