//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Sockets;
using nanoFramework.DependencyInjection;
using System;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Extension methods for setting up socket services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class SocketServerServiceCollectionExtension
    {
        /// <summary>
        /// Adds socket services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSocketServer(this IServiceCollection services)
        {
            return services.AddSocketServer(configure => { });
        }

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ServerOptionsAction"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSocketServer(this IServiceCollection services, ServerOptionsAction configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var options = new ServerOptions();

            configure(options);

            services.TryAdd(new ServiceDescriptor(typeof(IServerOptions), options));
            services.AddHostedService(typeof(SocketServerService));

            return services;
        }
    }
}