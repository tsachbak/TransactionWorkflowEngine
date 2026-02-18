using TransactionWorkflowEngine.Dtos;
using TransactionWorkflowEngine.Models;
using TransactionWorkflowEngine.Services.HistoryService;
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
        private readonly IHistoryService _historyService;

        public TransactionsHandler(IStatusesService statusesService,
                                   ITransactionsService transactionsService,
                                   ITransitionsService transitionsService,
                                   IHistoryService historyService)
        {
            _statusesService = statusesService;
            _transactionsService = transactionsService;
            _transitionsService = transitionsService;
            _historyService = historyService;
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

        public async Task<TransactionDto?> TransitionTransactionAsync(Guid transactionId, int toStatusId, string? reason, CancellationToken ct)
        {
            var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId, ct);
            if (transaction == null)
                return null;

            var fromStatusId = transaction.CurrentStatusId;

            // Validate requested transition against dynamic workflow configuration.
            var isTransitionAllowed = await _transitionsService.IsTransitionAllowedAsync(fromStatusId, toStatusId, ct);
            if (!isTransitionAllowed)
                throw new InvalidOperationException($"Transition from status '{fromStatusId}' to status with ID '{toStatusId}' is not allowed.");

            // Persist status update and history together to avoid partial workflow state.
            await _transactionsService.UpdateStatusWithHistoryAsync(transaction, toStatusId, reason, ct);

            return await GetTransactionByIdAsync(transactionId, ct);
        }

        public async Task<TransactionHistoryDto?> GetTransactionHistoryAsync(Guid transactionId, CancellationToken ct)
        {
            var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId, ct);
            if (transaction == null)
                return null;

            var historyItems = await _historyService.GetHistoryByTransactionIdAsync(transactionId, ct);

            return new TransactionHistoryDto
            {
                TransactionId = transaction.Id,
                Items = historyItems
            };
        }
    }
}
