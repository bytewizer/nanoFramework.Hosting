//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//


using System;
using System.IO;
using System.Collections;

namespace nanoFramework.Hosting.Configuration.FileExtensions
{
    /// <summary>
    /// Base class for file based <see cref="ConfigurationProvider"/>.
    /// </summary>
    public abstract class FileConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public FileConfigurationProvider(FileConfigurationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            Source = source;
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public FileConfigurationSource Source { get; }

        /// <summary>
        /// Generates a string representing this provider name and relevant details.
        /// </summary>
        /// <returns> The configuration name. </returns>
        public override string ToString()
            => $"{GetType().Name} for '{Source.Path}' ({(Source.Optional ? "Optional" : "Required")})";

        private void Load(bool reload)
        {
            IFileInfo file = Source.FileProvider?.GetFileInfo(Source.Path ?? string.Empty);
            if (file == null || !file.Exists)
            {
                if (Source.Optional || reload) // Always optional on reload
                {
                    Data = new Hashtable();
                }
                else
                {
                    //var error = new StringBuilder(SR.Format(SR.Error_FileNotFound, Source.Path));
                    //if (!string.IsNullOrEmpty(file?.PhysicalPath))
                    //{
                    //    error.Append(SR.Format(SR.Error_ExpectedPhysicalPath, file.PhysicalPath));
                    //}
                    //HandleException(ExceptionDispatchInfo.Capture(new FileNotFoundException(error.ToString())));
                }
            }
            else
            {
                static Stream OpenRead(IFileInfo fileInfo)
                {
                    if (fileInfo.PhysicalPath != null)
                    {
                        return new FileStream(
                            fileInfo.PhysicalPath,
                            FileMode.Open,
                            FileAccess.Read);
                    }

                    return fileInfo.CreateReadStream();
                }

                using Stream stream = OpenRead(file);
                try
                {
                    Load(stream);
                }
                catch (Exception ex)
                {
                    if (reload)
                    {
                        Data = new Hashtable();
                    }
                    //var exception = new InvalidDataException(SR.Format(SR.Error_FailedToLoad, file.PhysicalPath), ex);
                    //HandleException(ExceptionDispatchInfo.Capture(exception));
                }
            }
        }

        /// <summary>
        /// Loads the contents of the file at <see cref="Path"/>.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">Optional is <c>false</c> on the source and a
        /// directory cannot be found at the specified Path.</exception>
        /// <exception cref="FileNotFoundException">Optional is <c>false</c> on the source and a
        /// file does not exist at specified Path.</exception>
        /// <exception cref="InvalidDataException">An exception was thrown by the concrete implementation of the
        /// <see cref="Load()"/> method. Use the source <see cref="FileConfigurationSource.OnLoadException"/> callback
        /// if you need more control over the exception.</exception>
        public override void Load()
        {
            Load(reload: false);
        }

        /// <summary>
        /// Loads this provider's data from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public abstract void Load(Stream stream);

        private void HandleException(ExceptionDispatchInfo info)
        {
            bool ignoreException = false;
            if (Source.OnLoadException != null)
            {
                var exceptionContext = new FileLoadExceptionContext
                {
                    Provider = this,
                    Exception = info.SourceException
                };
                Source.OnLoadException.Invoke(exceptionContext);
                ignoreException = exceptionContext.Ignore;
            }
            if (!ignoreException)
            {
                info.Throw();
            }
        }
    }
}
