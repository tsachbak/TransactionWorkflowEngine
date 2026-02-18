using TransactionWorkflowEngine.Dtos;
using TransactionWorkflowEngine.Services.StatusesService;
using TransactionWorkflowEngine.Services.TransactionsService;
using TransactionWorkflowEngine.Services.TransitionsService;

namespace TransactionWorkflowEngine.Handlers.TransactionsHandler
{
    public class TransactionsHandler : ITransactionsHandler
    {
        private const string InitialStatusName = "CREATED";

        private readonly IStatusesService _statusesService;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransitionsService _transitionsService;

        public TransactionsHandler(IStatusesService statusesService, ITransactionsService transactionsService, ITransitionsService transitionsService)
        {
            _statusesService = statusesService;
            _transactionsService = transactionsService;
            _transitionsService = transitionsService;
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

        public async Task<AvailableTransitionsDto?> GetAvailableTransitionsAsync(Guid transactionId, CancellationToken ct)
        {
            var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId, ct);
            if (transaction == null)
                return null;

            var allowedNextStatuses = await _transitionsService.GetAllowedNextStatusesAsync(transaction.CurrentStatus.Id, ct);

            return new AvailableTransitionsDto
            {
                TransactionId = transaction.Id,
                CurrentStatus = new TransactionStatusDto
                {
                    Id = transaction.CurrentStatus.Id,
                    Name = transaction.CurrentStatus.Name
                },
                AllowedNextStatuses = allowedNextStatuses
                    .Select(s => new TransactionStatusDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToList()
            };
        }
    }
}
