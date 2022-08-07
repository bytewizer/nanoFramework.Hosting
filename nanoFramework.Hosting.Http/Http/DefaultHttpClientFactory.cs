//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Net.Http;

namespace nanoFramework.Hosting.Http
{
    internal class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly Hashtable _activeClients;

        public DefaultHttpClientFactory()
        {
            _activeClients = new Hashtable();
            _activeClients.Add(Options.DefaultName, new HttpClient());
        }

        public HttpClient CreateClient(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            return (HttpClient)_activeClients[$"{name}"];
        }

        internal void SetClient(object key, object value)
        {
            if (_activeClients.Contains(key))
            {
                _activeClients.Remove(key);
            };

            _activeClients.Add(key, value);
        }
    }
}
