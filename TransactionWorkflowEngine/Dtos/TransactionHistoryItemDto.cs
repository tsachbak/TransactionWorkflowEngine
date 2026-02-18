namespace TransactionWorkflowEngine.Dtos
{
    public sealed class TransactionHistoryItemDto
    {
        public int FromStatusId { get; set; }
        public string FromStatusName { get; set; } = string.Empty;

        public int ToStatusId { get; set; }
        public string ToStatusName { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
        public string? Reason { get; set; }
    }
}
