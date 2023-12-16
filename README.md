### What is Logger?

Logger is a solution for you to save error logs or information from your application.

### How do I get started?

First, you need to create an application key to save the logs, visit https://jbsn-logger.vercel.app

Then you can use the initialize method to enter your key:

```csharp
Logger.Initialize("186a788f-9ee9-46e1-a6ca-9fa960df5a17");
```

Then you can use the LogError or LogInfo methods to log your application:
```csharp
Logger.Log(LogLevel.Error, "Fetching data...", ex, Guid.NewGuid()); // Generic log
Logger.LogError("Fetching data...", ex, Guid.NewGuid()); // Log error
Logger.LogInfo("Fetching data...", null, Guid.NewGuid()); // Log info
```
It is possible to report an error message or an exception, in which case it will be logged as a text

For structured logs, enter your message and eventId, or exception and eventId.

The maximum character limit for a log is 8,000 character.

### Middleware

It is possible to save request data from your API using the LoggerRouteInfo class

```csharp
string? BodyRequest;
string? BodyResponse;
string Path;
string? QueryString;
TimeSpan? Duration;
int? StatusCode;
IEnumerable<KeyValuePair<string, StringValues>>? Headers;
```

#### Complete middleware example

Inject the logger middleware into your project
```csharp
app.InjectLoggerMiddleware();
```

Create the MiddlewareExtension
```csharp

public static class MiddlewareExtension
{
    public static IApplicationBuilder InjectLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}
```

Creates LoggerMiddleware
```csharp
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                var bodyRequest = await GetBodyRequest(httpContext);
                var memoryStreamBodyResponse = new MemoryStream();
                var bodyStream = httpContext.Response.Body;
                httpContext.Response.Body = memoryStreamBodyResponse;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await _next(httpContext);

                stopwatch.Stop();

                var path = httpContext.Request.Path.ToString().ToLower();
                var bodyResponse = await GetBodyResponse(httpContext, bodyStream);
                var statusCode = httpContext.Response.StatusCode;
                var queryString = httpContext.Request.QueryString.ToString();
                var level = statusCode >= 200 && statusCode <= 299 ? LoggerLevel.Info: LoggerLevel.Error;

                var routeInfo = new LoggerRouteInfoDto
                {
                    BodyRequest = bodyRequest,
                    BodyResponse = bodyResponse,
                    Duration = stopwatch.Elapsed,
                    Path = path,
                    QueryString = queryString,
                    StatusCode = statusCode,
                    Headers = httpContext.Request.Headers
                };

                _ = Logger.Log(level, routeInfo: routeInfo);
            }
            catch (Exception ex)
            {
                _ = Logger.LogError(exception: ex);
            }
        }

        protected async Task<string?> GetBodyRequest(HttpContext httpContext)
        {
            string? response = null;

            if (httpContext.Request.ContentLength > 0)
            {
                var memoryStream = new MemoryStream();
                await httpContext.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                response = await new StreamReader(memoryStream).ReadToEndAsync();
                memoryStream.Position = 0;
                httpContext.Request.Body = memoryStream;
            }

            return response;
        }

        protected async Task<string?> GetBodyResponse(HttpContext httpContext, Stream bodyStream)
        {
            string? response = null;

            if (httpContext.Response.Body != null && httpContext.Response.Body.Length > 0)
            {
                httpContext.Response.Body.Position = 0;
                response = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                httpContext.Response.Body.Position = 0;
                await httpContext.Response.Body.CopyToAsync(bodyStream);
                httpContext.Response.Body = bodyStream;
            }

            return response;
        }
    }
```

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [JbsnLogger](https://www.nuget.org/packages/JbsnLogger) from the package manager console:

```
PM> Install-Package JbsnLogger
```
Or from the .NET CLI as:
```
dotnet add package JbsnLogger
```

### License, etc.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).
