//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;
using System.Net.Http;

namespace nanoFramework.Hosting.Http
{
    /// <summary>
    /// Extension methods to configure an <see cref="IServiceCollection"/> for <see cref="IHttpClientFactory"/>.
    /// </summary>
    public static class HttpClientFactoryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHttpClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            services.TryAdd(new ServiceDescriptor(
                    typeof(IHttpClientFactory),
                    typeof(DefaultHttpClientFactory),
                    ServiceLifetime.Singleton)
                );

            //services.TryAdd(new ServiceDescriptor(
            //        typeof(ITypedHttpClientFactory),
            //        typeof(DefaultTypedHttpClientFactory),
            //        ServiceLifetime.Singleton)
            //    );

            return services;
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
        /// a named <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="HttpClient"/> to configure.</param>
        /// <remarks>
        /// <para>
        /// <see cref="HttpClient"/> instances that apply the provided configuration can be retrieved using
        /// <see cref="IHttpClientFactory.CreateClient(string)"/> and providing the matching name.
        /// </para>
        /// <para>
        /// Use <see cref="Options.DefaultName"/> as the name to configure the default client.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddHttpClient(this IServiceCollection services, string name)
        {
            return AddHttpClient(services, name, _ => { });
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
        /// a named <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
        /// <remarks>
        /// <para>
        /// <see cref="HttpClient"/> instances that apply the provided configuration can be retrieved using
        /// <see cref="IHttpClientFactory.CreateClient(string)"/> and providing the matching name.
        /// </para>
        /// <para>
        /// Use <see cref="Options.DefaultName"/> as the name to configure the default client.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddHttpClient(this IServiceCollection services, ClientAction configureClient)
        {
            return AddHttpClient(services, Options.DefaultName, configureClient);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
        /// a named <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="HttpClient"/> to configure.</param>
        /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
        /// <remarks>
        /// <para>
        /// <see cref="HttpClient"/> instances that apply the provided configuration can be retrieved using
        /// <see cref="IHttpClientFactory.CreateClient(string)"/> and providing the matching name.
        /// </para>
        /// <para>
        /// Use <see cref="Options.DefaultName"/> as the name to configure the default client.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddHttpClient(this IServiceCollection services, string name, ClientAction configureClient)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null)
            {
                throw new ArgumentNullException();
            }

            if (configureClient == null)
            {
                throw new ArgumentNullException();
            }

            var client = new HttpClient();

            configureClient(client);

            var httpClientFactory = new DefaultHttpClientFactory();
            httpClientFactory.SetClient(name, client);

            services.TryAdd(new ServiceDescriptor(
                    typeof(IHttpClientFactory),
                    httpClientFactory)
                );

            services.TryAdd(new ServiceDescriptor(
                    typeof(HttpClient),
                    client)
                );

            return services;
        }

        //public static IServiceCollection AddHttpClient(this IServiceCollection services, Type customType, string name, ClientAction configureClient)
        //{
        //    var client = new HttpClient();

        //    configureClient(client);

        //    var httpClientFactory = new DefaultHttpClientFactory();
        //    httpClientFactory.SetClient(name, client);

        //    services.TryAdd(new ServiceDescriptor(
        //            typeof(IHttpClientFactory),
        //            httpClientFactory)
        //        );

        //    services.TryAdd(new ServiceDescriptor(
        //            typeof(HttpClient),
        //            client)
        //        );


        //    return services;
        //}
    }
}
