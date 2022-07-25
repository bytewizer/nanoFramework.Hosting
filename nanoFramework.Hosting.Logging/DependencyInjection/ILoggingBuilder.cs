//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// An interface for configuring logging providers.
    /// </summary>
    public interface ILoggingBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Logging services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
