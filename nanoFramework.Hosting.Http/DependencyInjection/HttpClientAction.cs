//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Net.Http;

namespace nanoFramework.Hosting.Http
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="httpClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
    public delegate void HttpClientAction(HttpClient httpClient);
}
