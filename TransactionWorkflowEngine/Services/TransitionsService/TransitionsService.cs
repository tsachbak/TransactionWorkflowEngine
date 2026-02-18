using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;

namespace TransactionWorkflowEngine.Services.TransitionsService
{
    public class TransitionsService : ITransitionsService
    {
        private readonly AppDbContext _db;

        public TransitionsService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Models.TransactionStatus>> GetAllowedNextStatusesAsync(int fromStatusId, CancellationToken ct)
        {
            var allowedNextStatuses = await _db.TransactionStatusTransitions
                .AsNoTracking()
                .Where(t => t.FromStatusId == fromStatusId)
                .Select(t => t.ToStatus)
                .OrderBy(s => s.Id)
                .ToListAsync(ct);

            return allowedNextStatuses;
        }
    }
}
