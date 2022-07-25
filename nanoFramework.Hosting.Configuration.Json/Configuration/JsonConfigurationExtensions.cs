//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;

namespace nanoFramework.Hosting.Configuration.Json
{
    /// <summary>
    /// Extension methods for adding <see cref="JsonStreamConfigurationProvider"/>.
    /// </summary>
    public static class JsonConfigurationExtensions
    {
        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="stream">The <see cref="Stream"/> to read the json configuration data from.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonStream(this IConfigurationBuilder builder, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }

            return builder.Add(new JsonStreamConfigurationSource() { Stream = stream });
        }
    }
}

