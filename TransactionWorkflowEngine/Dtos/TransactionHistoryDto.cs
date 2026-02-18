namespace TransactionWorkflowEngine.Dtos
{
    public sealed class TransactionHistoryDto
    {
        public Guid TransactionId { get; set; }
        public List<TransactionHistoryItemDto> Items { get; set; } = new List<TransactionHistoryItemDto>();
    }
}
