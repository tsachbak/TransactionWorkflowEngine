using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.StatusesService
{
    /// <summary>
    /// Provides read operations for workflow statuses.
    /// </summary>
    public interface IStatusesService
    {
        /// <summary>
        /// Gets a status by name.
        /// </summary>
        /// <param name="name">Status name.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The status when found; otherwise null.</returns>
        Task<TransactionStatus?> GetStatusByNameAsync(string name, CancellationToken ct);
    }
}
