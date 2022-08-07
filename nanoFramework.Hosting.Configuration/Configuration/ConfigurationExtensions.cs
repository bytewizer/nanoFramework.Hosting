//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Hosting.Configuration
{
    /// <summary>
    /// <see cref="IConfiguration"/> extension methods.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Extracts the value with the specified key and converts it to a <see cref="string"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        public static string GetValue(this IConfiguration configuration, string key)
        {            
            return (string)configuration[key];
        }

        /// <summary>
        /// Extracts the value with the specified key and converts it to a <see cref="string"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        public static string GetValue(this IConfiguration configuration, string key, string defaultValue)
        {
            return (string)GetOrDefault(configuration, key, defaultValue);
        }

        /// <summary>
        /// Extracts the value with the specified key or sets to default value.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        public static object GetOrDefault(this IConfiguration configuration, string key, object defaultValue)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException();
            }

            var value = configuration[key];

            if (value == null)
            {
                return defaultValue;
            }

            return value;
        }
    }
}
