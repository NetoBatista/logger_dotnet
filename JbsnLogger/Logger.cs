using JbsnLogger.Constant;
using JbsnLogger.Dto;
using JbsnLogger.Enum;
using System.Net.Http.Json;

namespace JbsnLogger
{
    public static class Logger
    {
        private static string _baseAddress = "https://jbsn-logger.azurewebsites.net/record";

        private static string _applicationKey = string.Empty;

        /// <summary>
        /// Initializes the logger informing the application key as a parameter.
        /// </summary>
        public static void Initialize(string applicationKey)
        {
            _applicationKey = applicationKey;
        }

        /// <summary>
        /// Format and write an log message.
        /// </summary>
        public static Task Log(LoggerLevel logLevel, string? message = null, Exception? exception = null, Guid? eventId = null, LoggerRouteInfo? routeInfo = null)
        {
            switch (logLevel)
            {
                case LoggerLevel.Error:
                    return LogError(message, exception, eventId, routeInfo);
                default:
                    return LogInfo(message, exception, eventId, routeInfo);
            }
        }

        /// <summary>
        /// Format and write an error log message.
        /// </summary>
        public static Task LogError(string? message = null, Exception? exception = null, Guid? eventId = null, LoggerRouteInfo? routeInfo = null)
        {
            return Log(LogTypeConstant.Error, message, exception, eventId, routeInfo);
        }

        /// <summary>
        /// Format and write an informational log message.
        /// </summary>
        public static Task LogInfo(string? message = null, Exception? exception = null, Guid? eventId = null, LoggerRouteInfo? routeInfo = null)
        {
            return Log(LogTypeConstant.Info, message, exception, eventId, routeInfo);
        }

        /// <summary>
        /// Format and write an log message.
        /// </summary>
        private static async Task Log(string logType, string? message = null, Exception? exception = null, Guid? eventId = null, LoggerRouteInfo? routeInfo = null)
        {
            ValidateApplicationKey();
            var dataDto = LogTransform.Transform(message, exception, eventId, routeInfo);
            var recordDto = new LoggerRecord
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