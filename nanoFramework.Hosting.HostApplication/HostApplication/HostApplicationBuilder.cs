using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Pipeline.Builder;
using nanoFramework.Hosting.Configuration;
using nanoFramework.Hosting.Threading;
using nanoFramework.Hosting.Logging;
using System;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// A builder for hosted applications and services which helps manage configuration, logging and lifetime.
    /// </summary>
    public sealed class HostApplicationBuilder
    {
        private readonly HostBuilder _hostBuilder;
        private bool _hostBuilt;

        /// <summary>
        /// Initializes a new instance of <see cref="HostApplicationBuilder"/>.
        /// </summary>
        public HostApplicationBuilder()
           : this (null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HostApplicationBuilder"/>.
        /// </summary>
        public HostApplicationBuilder(HostApplicationOptions options)
        {
            options ??= new HostApplicationOptions();

            _hostBuilder = new HostBuilder();

            if (!options.DisableDefaults)
            {
                _hostBuilder.ConfigureServices(services =>
                {
                    services.AddLogging();
                    services
                    services.AddThreadPool();
                });
            }

            Configuration = new ConfigurationBuilder();
            Logging = new LoggingBuilder(Services);
        }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public IHostBuilder Host { get => _hostBuilder; }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostBuilder.Services;

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public IConfigurationBuilder Configuration { get; }

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging { get; }

        /// <summary>
        /// Builds the <see cref="HostApplication"/>.
        /// </summary>
        /// <returns>A configured <see cref="HostApplication"/>.</returns>
        public HostApplication Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException();
            }
            _hostBuilt = true;

            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(typeof(IApplicationBuilder), typeof(ApplicationBuilder));
            });

            _hostBuilder.ConfigureAppConfiguration((context, collection) =>
            {
                Configuration.AddInMemoryCollection();

                context.Configuration = Configuration.Build();

                collection.AddSingleton(typeof(IConfigurationRoot), context.Configuration);
                collection.AddSingleton(typeof(IConfiguration), context.Configuration);
            });

            var host = _hostBuilder.Build();

            return new HostApplication(host);
        }
    }
}