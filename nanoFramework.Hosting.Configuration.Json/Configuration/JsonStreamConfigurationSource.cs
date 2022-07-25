//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Configuration.Streams;

namespace nanoFramework.Hosting.Configuration.Json
{
    /// <summary>
    /// Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class JsonStreamConfigurationSource : StreamConfigurationSource
    {
        /// <summary>
        /// Builds the <see cref="JsonStreamConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An <see cref="JsonStreamConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
            => new JsonStreamConfigurationProvider(this);
    }
}

