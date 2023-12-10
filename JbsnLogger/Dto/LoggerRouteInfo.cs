using Microsoft.Extensions.Primitives;

namespace JbsnLogger.Dto
{
    public class LoggerRouteInfo
    {
        public string? BodyRequest { get; set; }
        public string? BodyResponse { get; set; }
        public string Path { get; set; } = string.Empty;
        public string? QueryString { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? StatusCode { get; set; }
        public IEnumerable<KeyValuePair<string, StringValues>>? Headers { get; set; }
    }
}
