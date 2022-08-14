namespace nanoFramework.Hosting
{
    /// <summary>
    /// Settings for constructing an <see cref="HostApplicationBuilder"/>.
    /// </summary>
    public sealed class HostApplicationOptions
    {
        /// <summary>
        /// If <see langword="false"/>, configures the <see cref="HostApplicationBuilder"/> instance with pre-configured defaults.
        /// </summary>

        public bool DisableDefaults { get; set; }

        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName { get; set; }
    }
}