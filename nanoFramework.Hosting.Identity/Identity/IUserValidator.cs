//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Provides an abstraction for user validation.
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Validates the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="manager">The <see cref="IdentityProvider"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        IdentityResult Validate(IdentityProvider manager, IIdentityUser user);
    }
}
