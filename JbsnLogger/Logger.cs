using JbsnLogger.Constant;
using JbsnLogger.Dto;
using JbsnLogger.Enum;
using System.Net.Http.Json;

namespace JbsnLogger
{
    public static class Logger
    {
        private static string _baseAddress = "https://jbsn-logger-record.azurewebsites.net/api/record";

        private static string _applicationKey = string.Empty;

        /// <summary>
        /// Initializes the logger informing the application key as a parameter.
        /// </summary>
        public static void Initialize(string applicationKey)
        {
            _applicationKey = applicationKey;
        }

        /// <summary>
        /// Formats and writes an log message.
        /// </summary>
        public static Task Log(LogLevel logLevel, string message, Exception? exception = null, Guid? eventId = null)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    return LogError(message, exception, eventId);
                default:
                    return LogInfo(message, exception, eventId);
            }
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        public static Task LogError(string message, Exception? exception = null, Guid? eventId = null)
        {
            return Log(LogTypeConstant.Error, message, exception, eventId);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        public static Task LogInfo(string message, Exception? exception = null, Guid? eventId = null)
        {
            return Log(LogTypeConstant.Info, message, exception, eventId);
        }

        /// <summary>
        /// Formats and writes an log message.
        /// </summary>
        private static async Task Log(string logType, string message, Exception? exception = null, Guid? eventId = null)
        {
            ValidateApplicationKey();
            var dataDto = LogTransform.Transform(message, exception, eventId);
            var recordDto = new LoggerRecordDTO
            {
                ApplicationKey = _applicationKey,
                Data = dataDto,
                Type = logType
            };
            var client = new HttpClient();
            await client.PostAsJsonAsync(_baseAddress, recordDto);
        }

        /// <summary>
        /// Validates the application key.
        /// </summary>
        private static void ValidateApplicationKey()
        {
            if (string.IsNullOrEmpty(_applicationKey))
            {
                throw new InvalidOperationException("No application key was provided, call Logger.Initialize({key})");
            }
        }
    }
}