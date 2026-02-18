namespace TransactionWorkflowEngine.Dtos
{
    public sealed class TransactionDto
    {
        public Guid Id { get; set; }
        public TransactionStatusDto CurrentStatus { get; set; } = new TransactionStatusDto();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
