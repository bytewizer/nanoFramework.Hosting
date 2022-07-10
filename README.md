[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Hosting&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Hosting) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Hosting&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Hosting) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.Hosting.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Hosting/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET nanoFramework Generic Host Library repository
The .NET nanoFramework Generic Host provides convenience methods for creating [dependency injection (DI)](https://github.com/nanoframework/nanoFramework.DependencyInjection/tree/main) application containers with preconfigured defaults.

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Hosting | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Hosting/_apis/build/status/nanoframework.Hosting?repoName=nanoframework%2FnanoFramework.Hosting&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Hosting/_build/latest?definitionId=56&repoName=nanoframework%2FnanoFramework.Hosting&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Hosting.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Hosting/) |

## Samples

[Hosting Samples](https://github.com/nanoframework/Samples/tree/main/samples/Hosting)

[Hosting Unit Tests](https://github.com/nanoframework/nanoFramework.Hosting/tree/main/tests)

## Generic Host
A Generic Host configures a DI application container as well as provides services in the DI container which handle the the application lifetime. When a host starts it calls *Start* on each implementation of IHostedService registered in the service container's collection of hosted services. In the application container all IHostedService object that inherent BackgroundService or SchedulerService have their *ExecuteAsync* methods called.

This API mirrors as close as possible the official .NET 
[Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host).

```csharp
using nanoFramework.Hosting;
using nanoFramework.DependencyInjection;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            IHost host = CreateHostBuilder().Build();
            
            // starts application and blocks the main calling thread 
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(BackgroundQueue));
                    services.AddHostedService(typeof(SensorService));
                    services.AddHostedService(typeof(DisplayService));
                    services.AddHostedService(typeof(CustomService));
                });
    }
}
```

## BackgroundService base class

Provides a base class for implementing a long running IHostedService. The method *ExecuteAsync* is called asynchronously to run the background service. Your implementation of *ExecuteAsync* should finish promptly when the *CancellationRequested* is fired in order to gracefully shut down the service.

```csharp
public class SensorService : BackgroundService
{
    protected override void ExecuteAsync()
    {
        while (!CancellationRequested)
        {
            // to allow other threads time to process include 
            // at least one millsecond sleep in loop
            Thread.Sleep(1);
        }
    }
}
```

## SchedulerService base class
 Provides a base class to schedule a thread making use of the [Timer](https://docs.nanoframework.net/api/System.Threading.Timer.html) running IHostedServce. The timer triggers at a specified time and interval the 'ExecuteAsync' method. The timer is disabled on Stop and disposed when the service container is disposed.

```csharp
public class DisplayService : SchedulerService
{
    // represents a timer control that involks ExecuteAsync at a 
    // specified interval of time repeatedly
    public DisplayService() : base(TimeSpan.FromSeconds(1)) {}

    protected override void ExecuteAsync(object state)
    {   
    }
}
```

## IHostedService interface

When you register an IHostedService the host builder will call the *Start* and *Stop* methods of IHostedService type during application start and stop respectively. You can create multiple implementations of IHostedService and register them at the ConfigureService method into the DI container. All hosted services will be started and stopped along with the application.

```csharp
public class CustomService : IHostedService
{
    public void Start() { }

    public void Stop() { }
}
```

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).