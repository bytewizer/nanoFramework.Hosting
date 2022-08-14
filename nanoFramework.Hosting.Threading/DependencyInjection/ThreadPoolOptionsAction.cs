//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Threading
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="configure">Specifies the thread pool options to configure.</param>
    public delegate void ThreadPoolOptionsAction(ThreadPoolOptions configure);
}
