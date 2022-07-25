//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Logging;
using nanoFramework.Logging.Debug;
using nanoFramework.DependencyInjection;
using System;

using Microsoft.Extensions.Logging;

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            return AddLogging(services, builder => { });
        }

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ILoggingBuilder"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services, LoggingAction configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var loggerFactory = new DebugLoggerFactory();
            LogDispatcher.LoggerFactory = loggerFactory;
            //LoggerExtensions.MessageFormatter = typeof(LoggerFormat).GetType().GetMethod("MessageFormatter");

            var logger = (DebugLogger)loggerFactory.GetCurrentClassLogger();

            configure(new LoggingBuilder(services));

            // using TryAdd prevents duplicate logging objects if AddLogging() is added more then once
            services.TryAdd(new ServiceDescriptor(typeof(ILogger), logger));
            services.TryAdd(new ServiceDescriptor(typeof(ILoggerFactory), loggerFactory));




            //services.AddOptions();

            //services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            //services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerFilterOptions>>(
            //    new DefaultLoggerLevelConfigureOptions(LogLevel.Information)));

            return services;
        }


        ///// <summary>
        ///// Adds logging services to the specified <see cref="IServiceCollection"/> 
        ///// that is enabled for <see cref="LogLevel"/> Information or higher.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        ///// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        //public static IServiceCollection AddLogging(this IServiceCollection services)
        //{
        //    return AddLogging(services, LogLevel.Information);
        //}

        ///// <summary>
        ///// Adds logging services to the specified <see cref="IServiceCollection"/>.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        ///// <param name="level">The <see cref="LogLevel"/> to set as the minimum.</param></param>
        ///// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        //public static IServiceCollection AddLogging(this IServiceCollection services, LogLevel level)
        //{
        //    if (services == null)
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    var loggerFactory = new DebugLoggerFactory();
        //    LogDispatcher.LoggerFactory = loggerFactory;

        //    var logger = (DebugLogger)loggerFactory.GetCurrentClassLogger();
        //    logger.MinLogLevel = level;

        //    // using TryAdd prevents duplicate logging objects if AddLogging() is added more then once
        //    services.TryAdd(new ServiceDescriptor(typeof(ILogger), logger));
        //    services.TryAdd(new ServiceDescriptor(typeof(ILoggerFactory), loggerFactory));

        //    return services;
        //}
    }
}
