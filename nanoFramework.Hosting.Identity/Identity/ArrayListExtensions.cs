//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Hosting.Identity
{
    /// <summary>
    /// Contains extension methods for <see cref="ArrayList"/>.
    /// </summary>
    internal static class ArrayListExtensions
    {
        /// <summary>
        /// The collection whose elements should be added to the end of the <see cref="ArrayList"/>.
        /// </summary>
        /// <param name="source">The source <see cref="ArrayList"/>.</param>
        /// <param name="collection">The current <see cref="ICollection"/></param>
        public static void AddRange(this ArrayList source, ICollection collection)
        {
            foreach(var item in collection)
            {
                source.Add(item);
            }
        }
    }
}
