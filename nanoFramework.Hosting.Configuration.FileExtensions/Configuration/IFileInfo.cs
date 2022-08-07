//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;

namespace nanoFramework.Hosting.Configuration.FileExtensions
{
    /// <summary>
    /// Represents a file in the given file provider.
    /// </summary>
    public interface IFileInfo
    {
        /// <summary>
        /// True if resource exists in the underlying storage system.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// The length of the file in bytes, or -1 for a directory or non-existing files.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// The path to the file, including the file name. Return null if the file is not directly accessible.
        /// </summary>
        string? PhysicalPath { get; }

        /// <summary>
        /// The name of the file or directory, not including any path.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// When the file was last modified
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// True for the case TryGetDirectoryContents has enumerated a sub-directory
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// Return file contents as readonly stream. Caller should dispose stream when complete.
        /// </summary>
        /// <returns>The file stream</returns>
        Stream CreateReadStream();
    }
}

