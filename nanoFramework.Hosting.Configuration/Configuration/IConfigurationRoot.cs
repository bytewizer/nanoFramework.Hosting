﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Hosting.Configuration
{
    /// <summary>
    /// Represents the root of an <see cref="IConfiguration"/> hierarchy.
    /// </summary>
    public interface IConfigurationRoot : IConfiguration
    {
        /// <summary>
        /// Force the configuration values to be reloaded from the underlying <see cref="IConfigurationProvider"/>s.
        /// </summary>
        void Reload();

        /// <summary>
        /// The <see cref="IConfigurationProvider"/>s for this configuration.
        /// </summary>
        IEnumerable Providers { get; }
    }
}
