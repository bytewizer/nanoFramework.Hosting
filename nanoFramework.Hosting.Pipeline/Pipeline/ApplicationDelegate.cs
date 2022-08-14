//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Pipeline.Builder;

namespace nanoFramework.Hosting.Pipeline
{
    /// <summary>
    /// Represents a function that can configure an application pipeline.
    /// </summary>
    public delegate void ApplicationDelegate(IApplicationBuilder builder);
}
