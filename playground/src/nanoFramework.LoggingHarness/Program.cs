using nanoFramework.DependencyInjection;
using nanoFramework.Hosting;
using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Logging.Debug;
using System;
using System.Diagnostics;
using System.Reflection;

namespace nanoFramework.TestHarness
{
    // nanoff --target M5Core2 --serialport COM9 --update

    public class Program
    {
        public static void Main()
        {
            //var builder = HostApplication.CreateBuilder();
            ////builder.Services.AddSocketServer();

            //var app = builder.Build();
            ////app.UseHttpResponse();
            //app.Logger.Log(LogLevel.Information, "Hello!!!");

            //Debug.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);

            //app.Run();


            var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .AddSocketServer((options, pipeline) => 
            {
                options.Listen(80);
                pipeline.UseHttpResponse();
            })
            .BuildServiceProvider();

            //var defaultLogger = (ILogger)serviceProvider.GetRequiredService(typeof(ILogger));
            //defaultLogger.Log(LogLevel.Information, "Default Hello!!!");

            var loggerFactory = (ILoggerFactory)serviceProvider.GetRequiredService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger("name");
            logger.Log(LogLevel.Information, "Hello!!!");
        }
    }
}