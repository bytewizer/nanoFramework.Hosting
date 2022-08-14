//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// A <see cref="ILoggerFactory"/> used to create instance of
    /// <see cref="NullLogger"/> that logs nothing.
    /// </summary>
    public class NullLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Returns the shared instance of <see cref="NullLoggerFactory"/>.
        /// </summary>
        public static NullLoggerFactory Instance { get; } = new NullLoggerFactory();

        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return NullLogger.Instance;
        }

        /// <inheritdoc />
        public void AddProvider(ILoggerProvider provider) { }

        /// <inheritdoc />
        public void Dispose() { }
    }
}
