namespace JbsnLogger.Dto
{
    internal class LoggerData
    {
        public Guid? EventId { get; set; }
        public string? Message { get; set; } = null;
        public LoggerRouteInfo? RouteInfo { get; set; }
        public LoggerErrorInfo? ErrorInfo { get; set; }
    }
}
