//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;
using System.Collections;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.Configuration;
using nanoFramework.Hosting.Configuration.Json;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class JsonConfigurationTest
    {
        [TestMethod]
        public void CanBuildValidJsonFromStreamProvider()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    Assert.Equal("value2", (string)context.Configuration["key2"]);
                    Assert.Equal("3", (string)context.Configuration["object1:property2:1"]);
                    Assert.Equal("started", (string)context.Configuration["test:start"]);
                    Assert.Equal("stopped", (string)context.Configuration["test:stop"]);
                    
                    context.Configuration["Key1"] = "NewValue1";
                })
                .ConfigureAppConfiguration((builder) =>
                {
                    var json = @"{""key1"":1,""key2"":""value2"",""object1"":{""property1"":""value1"",""property2"":[2,3,4,5,6,7]}}";

                    builder.AddJsonStream(StringToStream(json));
                    builder.AddInMemoryCollection(new Hashtable()
                    {
                        { "test:start", "started" },
                        { "test:stop", "stopped" }
                    });
                }).Build();

            var config = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

            Assert.Equal("NewValue1", (string)config["key1"]);
            Assert.Equal("5", (string)config["object1:property2:3"]);
            Assert.Equal("started", (string)config["test:start"]);
        }

        [TestMethod]
        public void CanLoadValidJsonFromStreamProvider()
        {
            var json = @"
            {
                ""firstname"": ""test"",
                ""test.last.name"": ""last.name"",
                    ""residential.address"": {
                        ""street.name"": ""Something street"",
                        ""zipcode"": ""12345""
                    }
            }";

            var config = new ConfigurationBuilder().AddJsonStream(StringToStream(json)).Build();
            Assert.Equal("test", (string)config["firstname"]);
            Assert.Equal("last.name", (string)config["test.last.name"]);
            Assert.Equal("Something street", (string)config["residential.address:STREET.name"]);
            Assert.Equal("12345", (string)config["residential.address:zipcode"]);
        }

        [TestMethod]
        public void LoadMethodCanHandleEmptyValue()
        {
            var json = @"
            {
                ""name"": """"
            }";

            var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
            jsonConfigSrc.Load(StringToStream(json));

            Assert.Equal(string.Empty, (string)jsonConfigSrc.Get("name"));
        }

        [TestMethod]
        public void NonObjectRootIsInvalid()
        {
            var json = @"""test""";

            Assert.Throws(typeof(FormatException), () =>
            {
                var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
                jsonConfigSrc.Load(StringToStream(json));
            });
        }

        [TestMethod]
        public void ThrowFormatExceptionWhenStreamIsEmpty()
        {
            var json = @"";

            Assert.Throws(typeof(FormatException), () =>
            {
                var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
                jsonConfigSrc.Load(StringToStream(json));
            });
        }

        [TestMethod]
        public void ThrowExceptionWhenUnexpectedEndFoundBeforeFinishParsing()
        {
            var json = @"{
                ""name"": ""test"",
                ""address"": {
                    ""street"": ""Something street"",
                    ""zipcode"": ""12345""
                }
            /* Missing a right brace here*/";

            Assert.Throws(typeof(FormatException), () =>
            {
                var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
                jsonConfigSrc.Load(StringToStream(json));
            });

        }

        [TestMethod]
        public void ThrowExceptionWhenMissingCurlyBeforeFinishParsing()
        {
            var json = @"
            {
              ""Data"": {
            ";

            Assert.Throws(typeof(FormatException), () =>
            {
                var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
                jsonConfigSrc.Load(StringToStream(json));
            });
        }

        //[TestMethod]
        //public void SupportAndIgnoreJsonComments()
        //{
        //    var json = @"/* Comments */
        //        {/* Comments */
        //        ""name"": /* Comments */ ""test"",
        //        ""address"": {
        //            ""street"": ""Something street"", /* Comments */
        //            ""zipcode"": ""12345""
        //        }
        //    }";

        //    var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
        //    jsonConfigSrc.Load(StringToStream(json));

        //    Assert.Equal("test", jsonConfigSrc.Get("name"));
        //    Assert.Equal("Something street", jsonConfigSrc.Get("address:street"));
        //    Assert.Equal("12345", jsonConfigSrc.Get("address:zipcode"));
        //}

        [TestMethod]
        public void SupportAndIgnoreTrailingCommas()
        {
            var json = @"
            {
                ""firstname"": ""test"",
                ""test.last.name"": ""last.name"",
                    ""residential.address"": {
                        ""street.name"": ""Something street"",
                        ""zipcode"": ""12345"",
                    },
            }";

            var jsonConfigSrc = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
            jsonConfigSrc.Load(StringToStream(json));

            Assert.Equal("test", (string)jsonConfigSrc.Get("firstname"));
            Assert.Equal("last.name", (string)jsonConfigSrc.Get("test.last.name"));
            Assert.Equal("Something street", (string)jsonConfigSrc.Get("residential.address:STREET.name"));
            Assert.Equal("12345", (string)jsonConfigSrc.Get("residential.address:zipcode"));
        }

        public static Stream StringToStream(string str)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
