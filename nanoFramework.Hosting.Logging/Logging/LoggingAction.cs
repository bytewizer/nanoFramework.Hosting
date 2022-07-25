//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Logging;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="builder">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
    public delegate void LoggingAction(ILoggingBuilder builder);
}
