//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//


using System;

using nanoFramework.Hosting.Sockets;
using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Pipeline.Builder;

namespace nanoFramework.Hosting
{
    public static class SocketServerServiceCollectionExtension
    {
        public static IServiceCollection AddSocketServer(this IServiceCollection services)
        {
            return services.AddSocketServer((configure, app) => { });
        }

        public static IServiceCollection AddSocketServer(this IServiceCollection services, ServerOptionsDelegate configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var options = new ServerOptions();
            var builder = new ApplicationBuilder();

            configure(options, builder);

            services.TryAdd(new ServiceDescriptor(typeof(ServerOptions), options));
            services.TryAdd(new ServiceDescriptor(typeof(ApplicationBuilder), options));
            services.AddHostedService(typeof(SocketServerService));

            return services;
        }
    }
}