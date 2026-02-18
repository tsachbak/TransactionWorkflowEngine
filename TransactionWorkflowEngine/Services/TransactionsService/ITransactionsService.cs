using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.TransactionsService
{
    /// <summary>
    /// Transactions Service Contract: Defines the operations related to transactions, such as creating new transactions with an initial status.
    /// </summary>
    public interface ITransactionsService
    {
        /// <summary>
        /// Get a transaction by its unique identifier.
        /// </summary>
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);

        /// <summary>
        /// Create a new transaction with the specified initial status.
        /// </summary>
        Task<Transaction> CreateTransactionAsync(int initialStatusId, CancellationToken ct);

        /// <summary>
        /// update the status of an existing transaction.
        /// </summary>
        Task UpdateStatusAsync(Guid transactionId, int newStatusId, CancellationToken ct);

        /// <summary>
        /// Updates transaction status and writes history in a single atomic save operation.
        /// </summary>
        Task UpdateStatusWithHistoryAsync(Transaction transaction, int toStatusId, string? reason, CancellationToken ct);
    }
}
