﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;

using System;

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Extensions for <see cref="IHostBuilder"/>.
    /// </summary>
    public static class IdentityHostBuilderExtensions
    {
        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configure">The delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureIdentity(this IHostBuilder hostBuilder, IdentityProviderAction configure)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException();
            }

            return hostBuilder.Configure((collection, services) =>
            {
                var identityProvider = (IdentityProvider)services.GetRequiredService(typeof(IIdentityProvider));
                
                configure(identityProvider);
            });
        }
    }
}
