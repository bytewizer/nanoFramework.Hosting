//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Logging
{
    internal readonly struct MessageLogger
    {
        public MessageLogger(ILogger logger, string category, LogLevel minLevel)
        {
            Logger = logger;
            Category = category;
            MinLevel = minLevel;
        }

        public ILogger Logger { get; }

        public string Category { get; }

        public LogLevel MinLevel { get; }

        public bool IsEnabled(LogLevel level)
        {
            if (level < MinLevel)
            {
                return false;
            }

            return true;
        }
    }
}
