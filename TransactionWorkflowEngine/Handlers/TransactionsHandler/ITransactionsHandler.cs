using TransactionWorkflowEngine.Dtos;

namespace TransactionWorkflowEngine.Handlers.TransactionsHandler
{
    /// <summary>
    /// Transactions Handler Contract: Defines the operations related to handling transactions.
    /// </summary>
    public interface ITransactionsHandler
    {
        /// <summary>
        /// Gets a transaction by its unique identifier.
        /// </summary>
        Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Creates a new transaction and return the created transaction details.
        /// </summary>
        Task<TransactionDto> CreateTransactionAsync(CancellationToken ct);

        /// <summary>
        /// Gets the available transitions for a given transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AvailableTransitionsDto?> GetAvailableTransitionsAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Transitions the specified transaction to a new status asynchronously.
        /// </summary>
        Task<TransactionDto?> TransitionTransactionAsync(Guid transactionId, int toStatusId, string? reason, CancellationToken ct);

        /// <summary>
        /// Get the transaction history for a given transaction, including all status changes and timestamps.
        /// </summary>
        Task<TransactionHistoryDto?> GetTransactionHistoryAsync(Guid transactionId, CancellationToken ct);
    }
}
