//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;
using System.Net.Http;

namespace nanoFramework.Hosting.Http
{
    internal class DefaultTypedHttpClientFactory : ITypedHttpClientFactory
    {
        private readonly IServiceProvider _services;

        public DefaultTypedHttpClientFactory(IServiceProvider services)
        {
            _services = services;
        }

        public object CreateClient(HttpClient httpClient, Type customeType)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException();
            }

            if (customeType == null)
            {
                throw new ArgumentNullException();
            }

            return ActivatorUtilities.CreateInstance(_services, customeType, new Type[] { typeof(HttpClient), });
        }
    }
}
