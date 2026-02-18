using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.TransactionsService
{
    /// <summary>
    /// Transactions Service Contract: Defines the operations related to transactions, such as creating new transactions with an initial status.
    /// </summary>
    public interface ITransactionsService
    {

        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);
        Task<Transaction> CreateTransactionAsync(int initialStatusId, CancellationToken ct);
    }
}
