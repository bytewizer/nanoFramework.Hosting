//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Pipeline.Builder;
using System;

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// Extensions for <see cref="IHostBuilder"/>.
    /// </summary>
    public static class SocketServerHostBuilderExtensions
    {
        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configure">The delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureSocketServer(this IHostBuilder hostBuilder, ApplicationBuilderAction configure)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException();
            }

            return hostBuilder.Configure((collection, services) =>
            {
                var applicationBuilder = new ApplicationBuilder(services);
                
                configure(applicationBuilder);
                
                collection.TryAdd(new ServiceDescriptor(typeof(ISocketServiceBuilder), applicationBuilder));
            });
        }
    }
}
