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
Logger.Log(LogLevel.Error, "/api/...", ex, Guid.NewGuid()); // Generic log
Logger.LogError("/api/...", ex, Guid.NewGuid()); // Log error
Logger.LogInfo("/api/...", null, Guid.NewGuid()); // Log info
```
It is possible to report an error message or an exception, in which case it will be logged as a text

For structured logs, enter your message and eventId, or exception and eventId.

The maximum character limit for a message is 8,000.


### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [AutoMapper](https://www.nuget.org/packages/AutoMapper/) from the package manager console:

```
PM> Install-Package 
```
Or from the .NET CLI as:
```
dotnet add package 
```

### License, etc.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).