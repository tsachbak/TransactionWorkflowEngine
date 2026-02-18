using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.StatusesService
{
    /// <summary>
    /// Statuses Service Contract: Defines the operations related to transaction statuses, such as retrieving status information by name.
    /// </summary>
    public interface IStatusesService
    {
        Task<TransactionStatus?> GetStatusByNameAsync(string name, CancellationToken ct);
    }
}
