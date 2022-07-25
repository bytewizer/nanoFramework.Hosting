﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="context">The context for the host builder.</param>
    /// <param name="builder">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
    public delegate void LoggingContextDelegate(HostBuilderContext context, ILoggingBuilder builder);
}