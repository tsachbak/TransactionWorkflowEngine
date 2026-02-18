using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.TransitionsService
{
    public interface ITransitionsService
    {
        Task<List<TransactionStatus>> GetAllowedNextStatusesAsync(int fromStatusId, CancellationToken ct);
    }
}
