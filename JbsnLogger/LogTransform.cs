
using JbsnLogger.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JbsnLogger
{
    internal static class LogTransform
    {
        internal static string Transform(
            string? message,
            Exception? exception = null,
            Guid? eventId = null,
            LoggerRouteInfo? routeInfo = null)
        {
            LoggerErrorInfo? errorInfo = null;
            if (exception != null)
            {
                errorInfo = new LoggerErrorInfo
                {
                    Exception = exception.Message,
                    StackTrace = exception.StackTrace,
                };
            }
            var dataDto = new LoggerData
            {
                EventId = eventId,
                Message = message,
                RouteInfo = routeInfo,
                ErrorInfo = errorInfo
            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            };

            return JsonSerializer.Serialize(dataDto, options);
        }
    }
}
