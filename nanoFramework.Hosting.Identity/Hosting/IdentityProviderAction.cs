//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Represents a method to configure <see cref="IdentityProvider"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="IIdentityProvider"/> configuration specific features.</param>
    public delegate void IdentityProviderAction(IIdentityProvider configure);
}