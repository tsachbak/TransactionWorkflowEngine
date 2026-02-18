using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Services.StatusesService
{
    public class StatusesService : IStatusesService
    {
        private readonly AppDbContext _db;

        public StatusesService(AppDbContext db)
        {
            _db = db;
        }

        public Task<TransactionStatus?> GetStatusByNameAsync(string name, CancellationToken ct)
        {
            return _db.TransactionStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.Name == name, ct);
        }
    }
}
