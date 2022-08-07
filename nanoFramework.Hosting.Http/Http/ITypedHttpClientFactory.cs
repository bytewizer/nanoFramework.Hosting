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
    /// A factory abstraction for a component that can create typed client instances with custom
    /// configuration for a given logical name.
    /// </summary>
    public interface ITypedHttpClientFactory
    {
        /// <summary>
        /// Creates a typed client given an associated <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="httpClient">
        /// An <see cref="HttpClient"/> created by the <see cref="IHttpClientFactory"/> for the named client
        /// associated with <paramref name="customeType"/>.
        /// </param>
        /// <param name="customeType"></param>
        /// <returns>An instance of <paramref name="customeType"/>.</returns>
        object CreateClient(HttpClient httpClient, Type customeType);
    }
}
