﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// A delegate which is executed when a update to the idenity store has occured.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    public delegate void StoreChangedHandler(object sender);
}