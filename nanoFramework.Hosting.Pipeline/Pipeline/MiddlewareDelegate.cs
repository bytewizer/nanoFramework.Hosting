//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Pipeline
{
    /// <summary>
    /// Represents a function that can process a pipeline middleware.
    /// </summary>
    public delegate IMiddleware MiddlewareDelegate();
}