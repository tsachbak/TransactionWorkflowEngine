using TransactionWorkflowEngine.Dtos;
using TransactionWorkflowEngine.Services.StatusesService;
using TransactionWorkflowEngine.Services.TransactionsService;

namespace TransactionWorkflowEngine.Handlers.TransactionsHandler
{
    public class TransactionsHandler : ITransactionsHandler
    {
        private const string InitialStatusName = "CREATED";

        private readonly IStatusesService _statusesService;
        private readonly ITransactionsService _transactionsService;

        public TransactionsHandler(IStatusesService statusesService, ITransactionsService transactionsService)
        {
            _statusesService = statusesService;
            _transactionsService = transactionsService;
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct)
        {
            var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId, ct);
            if (transaction == null)
                return null;

            return new TransactionDto
            {
                Id = transaction.Id,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                CurrentStatus = new TransactionStatusDto
                {
                    Id = transaction.CurrentStatus.Id,
                    Name = transaction.CurrentStatus.Name
                }
            };
        }

        public async Task<TransactionDto> CreateTransactionAsync(CancellationToken ct)
        {
            var initialStatus = await _statusesService.GetStatusByNameAsync(InitialStatusName, ct);
            if (initialStatus == null)
                throw new InvalidOperationException($"Initial status '{InitialStatusName}' not found.");

            var transaction = await _transactionsService.CreateTransactionAsync(initialStatus.Id, ct);

            return new TransactionDto
            {
                Id = transaction.Id,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                CurrentStatus = new TransactionStatusDto
                {
                    Id = transaction.CurrentStatus.Id,
                    Name = transaction.CurrentStatus.Name
                }
            };
        }
    }
}
