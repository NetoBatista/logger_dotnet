
using JbsnLogger.Dto;
using System.Text.Json;

namespace JbsnLogger
{
    internal static class LogTransform
    {
        internal static string Transform(string message, Exception? exception = null, Guid? eventId = null)
        {
            if (!string.IsNullOrEmpty(message) && exception == null && eventId == null)
            {
                return message;
            }

            if (exception != null && string.IsNullOrEmpty(message) && eventId == null)
            {
                return exception.Message;
            }

            var dataDto = new LoggetDataDto
            {
                EventId = eventId,
                Message = message,
                Exception = exception?.Message,
                StackTrace = exception?.StackTrace
            };

            return JsonSerializer.Serialize(dataDto);
        }
    }
}
