using System;
using System.Collections;

using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Pipeline;
using nanoFramework.Hosting.Pipeline.Builder;
using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// The host application used to configure the pipeline, and routes.
    /// </summary>
    public class HostApplication : IHost, IApplicationBuilder, IDisposable
    {
        private readonly IHost _host;

        /// <summary>
        /// Initializes a new instance of <see cref="HostApplicationBuilder"/>.
        /// </summary>
        internal HostApplication(IHost host)
        {
            _host = host;

            var loggerFactory = (ILoggerFactory)host.Services.GetRequiredService(typeof(ILoggerFactory));
            Logger = loggerFactory.CreateLogger("appname");
        }

        internal IApplicationBuilder ApplicationBuilder
            => (IApplicationBuilder)_host.Services.GetRequiredService(typeof(IApplicationBuilder));

        IServiceProvider IApplicationBuilder.ApplicationServices
        {
            get => ApplicationBuilder.ApplicationServices;
            set => ApplicationBuilder.ApplicationServices = value;
        }

        /// <inheritdoc />
        public Hashtable Properties => ApplicationBuilder.Properties;

        /// <summary>
        /// The application's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The default logger for the application.
        /// </summary>
        public ILogger Logger { get; }    

        /// <summary>
        /// The application's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration 
            => (IConfiguration)_host.Services.GetRequiredService(typeof(IConfiguration));

        /// <summary>
        /// Initializes a new instance of the <see cref="HostApplication"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="HostApplication"/>.</returns>
        public static HostApplication Create() => new HostApplicationBuilder().Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="HostApplicationBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="HostApplicationBuilder"/>.</returns>
        public static HostApplicationBuilder CreateBuilder() => new HostApplicationBuilder();

        /// <inheritdoc />
        public IApplication Build()
        {  
            return ApplicationBuilder.Build();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _host.Dispose(); 
        }

        /// <inheritdoc />
        public object GetProperty(string key)
        {
            return ApplicationBuilder.GetProperty(key);
        }

        /// <inheritdoc />
        public void SetProperty(string key, object value)
        {
            ApplicationBuilder.SetProperty(key, value);
        }

        /// <inheritdoc />
        public bool TryGetProperty(string key, out object value)
        {
            return ApplicationBuilder.TryGetProperty(key, out value);
        }

        /// <inheritdoc />
        public void Start()
        {
            _host.Start();
        }


        /// <inheritdoc />
        public void Stop()
        {
            _host.Stop();
        }

        /// <inheritdoc />
        public IApplicationBuilder Use(Type serviceType)
        {
            return ApplicationBuilder.Use(serviceType);
        }

        /// <inheritdoc />
        public IApplicationBuilder Use(Type serviceType, params object[] args)
        {
            return ApplicationBuilder.Use(serviceType, args);
        }

        /// <inheritdoc />
        public IApplicationBuilder Use(IMiddleware middleware)
        {
            return ApplicationBuilder.Use(middleware);
        }

        /// <inheritdoc />
        public IApplicationBuilder Use(InlineDelegate middleware)
        {
            return ApplicationBuilder.Use(middleware);
        }
    }
}