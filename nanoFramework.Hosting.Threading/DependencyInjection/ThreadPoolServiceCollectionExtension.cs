//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;
using System.Threading;

namespace nanoFramework.Hosting.Threading
{
    /// <summary>
    /// Extension methods for the <see cref="ThreadPool"/>.
    /// </summary>
    public static class ThreadPoolServiceCollectionExtension
    {
        /// <summary>
        /// Configures a pool of threads that can be used to execute short running tasks.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        public static IServiceCollection AddThreadPool(this IServiceCollection services)
        {
            return AddThreadPool(services, configure => { });
        }

        /// <summary>
        /// Configures a pool of threads that can be used to execute short running tasks.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <param name="configure">Specifies the thread pool options to configure.</param>
        public static IServiceCollection AddThreadPool(this IServiceCollection services, ThreadPoolOptionsAction configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var options = new ThreadPoolOptions();
            
            configure(options);

            ThreadPool.SetMinThreads(options.MinThreads);
            ThreadPool.SetMaxThreads(options.MaxThreads);

            return services.AddHostedService(typeof(TheadPoolService));
        }
    }
}