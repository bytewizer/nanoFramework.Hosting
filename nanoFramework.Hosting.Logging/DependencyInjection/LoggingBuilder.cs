//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting.Logging
{
    internal sealed class LoggingBuilder : ILoggingBuilder
    {
        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
