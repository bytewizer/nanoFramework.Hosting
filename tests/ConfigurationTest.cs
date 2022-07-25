//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.Configuration;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void LoadAndCombineKeyValuePairsFromDifferentConfigurationProviders()
        {
            var dic1 = new Hashtable()
            {
                {"Mem1:KeyInMem1", "ValueInMem1"}
            };
            var dic2 = new Hashtable()
            {
                {"Mem2:KeyInMem2", "ValueInMem2"}
            };
            var dic3 = new Hashtable()
            {
                {"Mem3:KeyInMem3", "ValueInMem3"}
            };

            var memConfigSrc1 = new MemoryConfigurationSource { InitialData = dic1 };
            var memConfigSrc2 = new MemoryConfigurationSource { InitialData = dic2 };
            var memConfigSrc3 = new MemoryConfigurationSource { InitialData = dic3 };

            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.Add(memConfigSrc1);
            configurationBuilder.Add(memConfigSrc2);
            configurationBuilder.Add(memConfigSrc3);

            var config = configurationBuilder.Build();

            var memVal1 = config["mem1:keyinmem1"];
            var memVal2 = config["Mem2:KeyInMem2"];
            var memVal3 = config["MEM3:KEYINMEM3"];

            Assert.Equal("ValueInMem1", memVal1);
            Assert.Equal("ValueInMem2", memVal2);
            Assert.Equal("ValueInMem3", memVal3);

            Assert.Equal("ValueInMem1", config["mem1:keyinmem1"]);
            Assert.Equal("ValueInMem2", config["Mem2:KeyInMem2"]);
            Assert.Equal("ValueInMem3", config["MEM3:KEYINMEM3"]);
            Assert.Null(config["NotExist"]);
        }

        [TestMethod]
        public void NewConfigurationProviderOverridesOldOneWhenKeyIsDuplicated()
        {
            var dic1 = new Hashtable()
                {
                    {"Key1:Key2", "ValueInMem1"}
                };
            var dic2 = new Hashtable()
                {
                    {"Key1:Key2", "ValueInMem2"}
                };
            var memConfigSrc1 = new MemoryConfigurationSource { InitialData = dic1 };
            var memConfigSrc2 = new MemoryConfigurationSource { InitialData = dic2 };

            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.Add(memConfigSrc1);
            configurationBuilder.Add(memConfigSrc2);

            var config = configurationBuilder.Build();

            Assert.Equal("ValueInMem2", config["Key1:Key2"]);
        }

        [TestMethod]
        public void SetValueThrowsExceptionNoSourceRegistered()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var config = configurationBuilder.Build();

            Assert.Throws(typeof(InvalidOperationException),() => config["Title"] = "Welcome");
        }

        [TestMethod]
        public void SettingValueUpdatesAllConfigurationProviders()
        {
            var dict = new Hashtable()
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };

            var memConfigSrc1 = new TestMemorySourceProvider(dict);
            var memConfigSrc2 = new TestMemorySourceProvider(dict);
            var memConfigSrc3 = new TestMemorySourceProvider(dict);

            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.Add(memConfigSrc1);
            configurationBuilder.Add(memConfigSrc2);
            configurationBuilder.Add(memConfigSrc3);

            var config = configurationBuilder.Build();

            config["Key1"] = "NewValue1";
            config["Key2"] = "NewValue2";

            var memConfigProvider1 = memConfigSrc1.Build(configurationBuilder);
            var memConfigProvider2 = memConfigSrc2.Build(configurationBuilder);
            var memConfigProvider3 = memConfigSrc3.Build(configurationBuilder);

            Assert.Equal("NewValue1", config["Key1"]);
            Assert.Equal("NewValue1", memConfigProvider1.Get("Key1"));
            Assert.Equal("NewValue1", memConfigProvider2.Get("Key1"));
            Assert.Equal("NewValue1", memConfigProvider3.Get("Key1"));
            Assert.Equal("NewValue2", config["Key2"]);
            Assert.Equal("NewValue2", memConfigProvider1.Get("Key2"));
            Assert.Equal("NewValue2", memConfigProvider2.Get("Key2"));
            Assert.Equal("NewValue2", memConfigProvider3.Get("Key2"));
        }

        [TestMethod]
        public void NewConfigurationRootMayBeBuiltFromExistingWithDuplicateKeys()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Hashtable()
                    {
                        {"keya:keyb", "valueA"},
                    })
                .AddInMemoryCollection(new Hashtable()
                    {
                        {"KEYA:KEYB", "valueB"}
                    })
                .Build();
                        
            Assert.Equal("valueB", configurationRoot["keya:keyb"]);
        }

        [TestMethod]
        public void AfterBuildDefaultConfigurationIsAvailable()
        {
            var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService(typeof(HostedService));

                Assert.Equal("started", context.Configuration["Test:Start"]);
                Assert.Equal("stopped", context.Configuration["Test:Stop"]);
            })
            .ConfigureAppConfiguration((builder) =>
            {
                builder.AddInMemoryCollection(new Hashtable()
                    {
                        { "Test:Start", "started" },
                    });
                builder.AddInMemoryCollection(new Hashtable()
                    {
                        { "Test:Stop", "stopped" }
                    });
            }).Build();

            Assert.NotNull(host.Services.GetService(typeof(IConfiguration)));
            Assert.NotNull(host.Services.GetService(typeof(IConfigurationRoot)));

            var service = (HostedService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.Equal("started", service.Started);
            
            host.Stop();
            Assert.Equal("stopped", service.Stopped);
        }

        [TestMethod]
        public void CreateDefaultBuilderStartsWithNullConfiguration()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    Assert.Null(context.Configuration);
                });
        }

        [TestMethod]
        public void CreateDefaultBuilderWithAppConfiguration()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    Assert.NotNull(context.Configuration);

                    context.Configuration["Key1"] = "NewValue1";
                    context.Configuration["Key2"] = "NewValue2";
                })
                .ConfigureAppConfiguration((builder) =>
                {
                    builder.AddInMemoryCollection();
                }).Build();

            var config = (IConfiguration)host.Services.GetService(typeof(IConfiguration));
            
            Assert.Equal("NewValue1", config["key1"]);
            Assert.Equal("NewValue2", config["key2"]);
        }

        private class TestMemorySourceProvider : MemoryConfigurationProvider, IConfigurationSource
        {
            public TestMemorySourceProvider(Hashtable initialData)
                : base(new MemoryConfigurationSource { InitialData = initialData })
            { }

            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                return this;
            }
        }

        public class HostedService : IHostedService
        {
            private readonly IConfiguration _config;
            private readonly IConfigurationRoot _configRoot;

            public HostedService(IConfiguration config, IConfigurationRoot configRoot)
            {
                _config = config;
                _configRoot = configRoot;   
            }

            public string Started { get; set; }
            public string Stopped { get; set; }

            public void Start()
            {
                _configRoot.Reload();
                Started = _config["test:start"];
            }

            public void Stop()
            {
                Stopped = _config["test:stop"];
            }
        }
    }

    public static class ConfigurationProviderExtensions
    {
        public static string Get(this IConfigurationProvider provider, string key)
        {
            string value;

            if (!provider.TryGet(key, out value))
            {
                throw new InvalidOperationException("Key not found");
            }

            return value;
        }
    }
}
