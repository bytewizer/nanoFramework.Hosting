﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Hosting.Configuration
{
    /// <summary>
    /// Represents in-memory data as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class MemoryConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// The initial key value configuration pairs.
        /// </summary>
        public Hashtable InitialData { get; set; }

        /// <summary>
        /// Builds the <see cref="MemoryConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="MemoryConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MemoryConfigurationProvider(this);
        }
    }
}

