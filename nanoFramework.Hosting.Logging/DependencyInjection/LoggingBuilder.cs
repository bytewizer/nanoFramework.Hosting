using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting.Logging
{
    public sealed class LoggingBuilder : ILoggingBuilder
    {
        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}