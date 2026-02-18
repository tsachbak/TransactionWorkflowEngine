namespace TransactionWorkflowEngine.Dtos
{
    public sealed class TransitionRequestDto
    {
        public int ToStatusId { get; set; }
        public string? Reason { get; set; }
    }
}
