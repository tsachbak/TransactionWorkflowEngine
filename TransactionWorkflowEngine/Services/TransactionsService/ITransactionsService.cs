using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.TransactionsService
{
    /// <summary>
    /// Provides transaction persistence operations used by the workflow layer.
    /// </summary>
    public interface ITransactionsService
    {
        /// <summary>
        /// Gets a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The transaction when found; otherwise null.</returns>
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Creates a new transaction with the specified initial status.
        /// </summary>
        /// <param name="initialStatusId">Initial status identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created transaction entity.</returns>
        Task<Transaction> CreateTransactionAsync(int initialStatusId, CancellationToken ct);

        /// <summary>
        /// Updates the current status of an existing transaction.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="newStatusId">Target status identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        Task UpdateStatusAsync(Guid transactionId, int newStatusId, CancellationToken ct);

        /// <summary>
        /// Updates status and writes a history entry in one atomic persistence operation.
        /// </summary>
        /// <param name="transaction">Tracked transaction entity to update.</param>
        /// <param name="toStatusId">Target status identifier.</param>
        /// <param name="reason">Optional business reason for the status change.</param>
        /// <param name="ct">Cancellation token.</param>
        Task UpdateStatusWithHistoryAsync(Transaction transaction, int toStatusId, string? reason, CancellationToken ct);
    }
}
