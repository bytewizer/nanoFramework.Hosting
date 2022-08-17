//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds identity services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            return AddIdentity(services, new PasswordHasher(), null, null);
        }

        /// <summary>
        /// Adds identity services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services, string user, byte[] password)
        {
            return AddIdentity(services, new IdentityUser(user), password);
        }

        /// <summary>
        /// Adds identity services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services, IPasswordHasher passwordHasher)
        {
            return AddIdentity(services, passwordHasher, null, null);
        }

        /// <summary>
        /// Adds identity services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services, IIdentityUser user, byte[] password)
        {
            return AddIdentity(services, new PasswordHasher(), user, password);
        }

        /// <summary>
        /// Adds identity services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services, IPasswordHasher passwordHasher, IIdentityUser user, byte[] password)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            var identityProvider = new IdentityProvider(passwordHasher);

            if (user != null || password != null)
            {
                identityProvider.Create(user, password);
            }

            services.TryAdd(new ServiceDescriptor(
                typeof(IIdentityProvider),
                identityProvider)
            );

            return services;
        }
    }
}