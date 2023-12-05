namespace JbsnLogger.Dto
{
    internal class LoggetDataDto
    {
        public Guid? EventId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; } = string.Empty;
        public string? StackTrace { get; set; } = string.Empty;
    }
}
