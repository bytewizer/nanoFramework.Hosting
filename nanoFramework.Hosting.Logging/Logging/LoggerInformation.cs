//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Hosting.Logging

{
    internal readonly struct LoggerInformation
    {
        public LoggerInformation(ILoggerProvider provider, string category)
            : this()
        {
            ProviderType = provider.GetType();
            Logger = provider.CreateLogger(category);
            Category = category;
        }

        public ILogger Logger { get; }

        public string Category { get; }

        public Type ProviderType { get; }
    }
}
