namespace TransactionWorkflowEngine.Dtos
{
    public sealed class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string? Instance { get; set; }
    }
}
