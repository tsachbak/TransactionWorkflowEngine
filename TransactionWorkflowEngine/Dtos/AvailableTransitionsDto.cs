namespace TransactionWorkflowEngine.Dtos
{
    public sealed class AvailableTransitionsDto
    {
        public Guid TransactionId { get; set; }
        public TransactionStatusDto CurrentStatus { get; set; } = new TransactionStatusDto();
        public List<TransactionStatusDto> AllowedNextStatuses { get; set; } = new List<TransactionStatusDto>();
    }
}
