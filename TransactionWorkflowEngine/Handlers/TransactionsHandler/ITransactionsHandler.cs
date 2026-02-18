using TransactionWorkflowEngine.Dtos;

namespace TransactionWorkflowEngine.Handlers.TransactionsHandler
{
    /// <summary>
    /// Coordinates transaction workflow use cases for the API layer.
    /// </summary>
    public interface ITransactionsHandler
    {
        /// <summary>
        /// Gets a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The transaction when found; otherwise null.</returns>
        Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Creates a new transaction in the initial workflow status.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created transaction.</returns>
        Task<TransactionDto> CreateTransactionAsync(CancellationToken ct);

        /// <summary>
        /// Gets the currently allowed next statuses for a transaction.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Available transitions when transaction exists; otherwise null.</returns>
        Task<AvailableTransitionsDto?> GetAvailableTransitionsAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Executes a status transition when the workflow allows it.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="toStatusId">Target status identifier.</param>
        /// <param name="reason">Optional business reason for the status change.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated transaction when successful; otherwise null.</returns>
        Task<TransactionDto?> TransitionTransactionAsync(Guid transactionId, int toStatusId, string? reason, CancellationToken ct);

        /// <summary>
        /// Gets the status transition history for a transaction.
        /// </summary>
        /// <param name="transactionId">Transaction identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The transition history when transaction exists; otherwise null.</returns>
        Task<TransactionHistoryDto?> GetTransactionHistoryAsync(Guid transactionId, CancellationToken ct);
    }
}
