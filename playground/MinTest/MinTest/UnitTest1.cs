using nanoFramework.TestFramework;
using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Logging.Debug;
using System;

namespace MinTest
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .BuildServiceProvider();

            Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILogger)));
            Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILoggerFactory)));
            Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILoggerProvider)));
            Assert.IsType(typeof(DebugLoggerProvider), serviceProvider.GetRequiredService(typeof(ILoggerProvider)));

            var options = (LoggerFilterOptions)serviceProvider.GetRequiredService(typeof(LoggerFilterOptions));

            Assert.NotNull(options);
            Assert.True(options.MinLevel == LogLevel.Information);

            var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger("name");
            logger.Log(LogLevel.Information, "Hello!!!");

        }
    }
}
