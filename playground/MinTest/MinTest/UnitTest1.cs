using nanoFramework.TestFramework;
using nanoFramework.Hosting.Identity;
using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Logging.Debug;
using System;



namespace MinTest
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .BuildServiceProvider();

            //Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILogger)));
            Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILoggerFactory)));
            Assert.NotNull(serviceProvider.GetRequiredService(typeof(ILoggerProvider)));
            Assert.IsType(typeof(DebugLoggerProvider), serviceProvider.GetRequiredService(typeof(ILoggerProvider)));

            var options = (LoggerFilterOptions)serviceProvider.GetRequiredService(typeof(LoggerFilterOptions));

            Assert.NotNull(options);
            Assert.True(options.MinLevel == LogLevel.Information);

            var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger("name");
            logger.Log(LogLevel.Information, "Hello!!!");

        }

        [TestMethod]
        public void IdentityTest()
        {
            string username = "b.smith";
            string password = "password";

            var serviceProvider = new ServiceCollection()
                .AddIdentity(username, password)
                .BuildServiceProvider();

            var identityProvider = (IdentityProvider)serviceProvider.GetRequiredService(typeof(IIdentityProvider));

            Assert.NotNull(identityProvider);

            var user = identityProvider.FindByName(username);

            Assert.Equal(username, user.UserName);
            Assert.True(identityProvider.VerifyPassword(user, password).Succeeded);
        }

        [TestMethod]
        public void IdentityWithCustomUserTest()
        {
            var user = new User("b.smith", "Bob", "Smith");
            string password = "password";

            var serviceProvider = new ServiceCollection()
                .AddIdentity(user, password)
                .BuildServiceProvider();

            var identityProvider = (IdentityProvider)serviceProvider.GetRequiredService(typeof(IIdentityProvider));

            Assert.NotNull(identityProvider);

            var results = identityProvider.FindByName(user.UserName);

            Assert.Equal(results.UserName, user.UserName);
            Assert.True(identityProvider.VerifyPassword(user, password).Succeeded);
        }
    }

    public class User : IIdentityUser
    {
        public User(string name, string firstName, string lastName)
        {
            Id = Guid.NewGuid().ToString();
            UserName = name;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public object Metadata { get; set; }
    }
}
