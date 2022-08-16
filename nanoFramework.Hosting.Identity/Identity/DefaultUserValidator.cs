﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Implements identity user name validation.
    /// </summary>
    public class DefaultUserValidator : IUserValidator
    {
        /// <summary>
        /// Returns a <see cref="IdentityResult"/> indicating the result of a password validation comparison.
        /// </summary>
        /// <param name="manager">Provides the APIs for managing user in a persistence store.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <returns>A <see cref="IdentityResult"/> indicating the result of a password hash comparison.</returns>
        public IdentityResult Validate(IdentityProvider manager, IIdentityUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException();
            }

            ArrayList errors = null;

            if (user.UserName.Length < 6)
            {
                errors ??= new ArrayList();
                errors.Add(new Exception("User name must be at least 6 characters long"));
            }

            if (errors != null)
            {
                return IdentityResult.Failed(errors);
            }

            return IdentityResult.Success;
        }
    }
}