//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;
using System.Collections;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.Configuration;
using nanoFramework.Hosting.Configuration.Ini;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class IniConfigurationTest
    {
        [TestMethod]
        public void CanBuildValidIniFromStreamProvider()
        {
            var ini = "[DefaultConnection]\n" +
                      "ConnectionString=\"TestConnectionString\"\n" +
                      "Provider=\"SqlClient\"\n" +
                      "[Data:Inventory]\n" +
                      "ConnectionString=\"AnotherTestConnectionString\"\n" +
                      "Provider=\"MySql\"";

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    Assert.Equal("AnotherTestConnectionString", context.Configuration["Data:Inventory:ConnectionString"]);
                    Assert.Equal("TestConnectionString", context.Configuration["DefaultConnection:ConnectionString"]);
                    Assert.Equal("started", context.Configuration["test:start"]);
                    Assert.Equal("stopped", context.Configuration["test:stop"]);
                })
                .ConfigureAppConfiguration((builder) =>
                {
                    builder.AddIniStream(StringToStream(ini));
                    builder.AddInMemoryCollection(new Hashtable()
                    {
                        { "test:start", "started" },
                        { "test:stop", "stopped" }
                    });
                });

            host.Build();
        }

        [TestMethod]
        public void CanLoadValidIniFromStreamProvider()
        {
            var ini = "[DefaultConnection]\n" +
                      "ConnectionString=\"TestConnectionString\"\n" +
                      "Provider=\"SqlClient\"\n" +
                      "[Data:Inventory]\n" +
                      "ConnectionString=\"AnotherTestConnectionString\"\n" +
                      "Provider=\"MySql\"";

            var config = new ConfigurationBuilder()
                .AddIniStream(StringToStream(ini))
                .Build();

            Assert.Equal("TestConnectionString", config["DefaultConnection:ConnectionString"]);
            Assert.Equal("SqlClient", config["DefaultConnection:Provider"]);
            Assert.Equal("AnotherTestConnectionString", config["Data:Inventory:ConnectionString"]);
            Assert.Equal("MySql", config["Data:Inventory:Provider"]);
        }

        [TestMethod]
        public void LoadKeyValuePairsFromValidIniFileWithQuotedValues()
        {
            var ini = "[DefaultConnection]\n" +
                      "ConnectionString=\"TestConnectionString\"\n" +
                      "Provider=\"SqlClient\"\n" +
                      "[Data:Inventory]\n" +
                      "ConnectionString=\"AnotherTestConnectionString\"\n" +
                      "Provider=\"MySql\"";
            
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("TestConnectionString", iniConfigSrc.Get("DefaultConnection:ConnectionString"));
            Assert.Equal("SqlClient", iniConfigSrc.Get("DefaultConnection:Provider"));
            Assert.Equal("AnotherTestConnectionString", iniConfigSrc.Get("Data:Inventory:ConnectionString"));
            Assert.Equal("MySql", iniConfigSrc.Get("Data:Inventory:Provider"));
        }

        [TestMethod]
        public void DoubleQuoteIsPartOfValueIfNotPaired()
        {
            var ini = "[ConnectionString]\n" +
                      "DefaultConnection=\"TestConnectionString\n" +
                      "Provider=SqlClient\"";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("\"TestConnectionString", iniConfigSrc.Get("ConnectionString:DefaultConnection"));
            Assert.Equal("SqlClient\"", iniConfigSrc.Get("ConnectionString:Provider"));
        }

        [TestMethod]
        public void DoubleQuoteIsPartOfValueIfAppearInTheMiddleOfValue()
        {
            var ini = "[ConnectionString]\n" +
                      "DefaultConnection=Test\"Connection\"String\n" +
                      "Provider=Sql\"Client";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("Test\"Connection\"String", iniConfigSrc.Get("ConnectionString:DefaultConnection"));
            Assert.Equal("Sql\"Client", iniConfigSrc.Get("ConnectionString:Provider"));
        }

        [TestMethod]
        public void LoadKeyValuePairsFromValidIniFileWithoutSectionHeader()
        {
            var ini = @"
            DefaultConnection:ConnectionString=TestConnectionString
            DefaultConnection:Provider=SqlClient
            Data:Inventory:ConnectionString=AnotherTestConnectionString
            Data:Inventory:Provider=MySql
            ";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("TestConnectionString", iniConfigSrc.Get("DefaultConnection:ConnectionString"));
            Assert.Equal("SqlClient", iniConfigSrc.Get("DefaultConnection:Provider"));
            Assert.Equal("AnotherTestConnectionString", iniConfigSrc.Get("Data:Inventory:ConnectionString"));
            Assert.Equal("MySql", iniConfigSrc.Get("Data:Inventory:Provider"));
        }

        [TestMethod]
        public void SupportAndIgnoreIniComments()
        {
            var ini = @"
            ; Comments
            [DefaultConnection]
            # Comments
            ConnectionString=TestConnectionString
            / Comments
            Provider=SqlClient
            [Data:Inventory]
            ConnectionString=AnotherTestConnectionString
            Provider=MySql
            ";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("TestConnectionString", iniConfigSrc.Get("DefaultConnection:ConnectionString"));
            Assert.Equal("SqlClient", iniConfigSrc.Get("DefaultConnection:Provider"));
            Assert.Equal("AnotherTestConnectionString", iniConfigSrc.Get("Data:Inventory:ConnectionString"));
            Assert.Equal("MySql", iniConfigSrc.Get("Data:Inventory:Provider"));
        }

        [TestMethod]
        public void ShouldRemoveLeadingAndTrailingWhiteSpacesFromKeyAndValue()
        {
            var ini = "[section]\n" +
                      " \t key \t = \t value\t ";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());
            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("value", iniConfigSrc.Get("section:key"));
        }

        [TestMethod]
        public void ShouldRemoveLeadingAndTrailingWhiteSpacesFromSectionName()
        {
            var ini = "[ \t section \t ]\n" +
                      "key=value";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());
            iniConfigSrc.Load(StringToStream(ini));

            Assert.Equal("value", iniConfigSrc.Get("section:key"));
        }

        [TestMethod]
        public void ThrowExceptionWhenFoundInvalidLine()
        {
            var ini = "ConnectionString";
            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            Assert.Throws(typeof(FormatException), () => iniConfigSrc.Load(StringToStream(ini)));
        }

        [TestMethod]
        public void ThrowExceptionWhenFoundBrokenSectionHeader()
        {
            var ini = "[ \t section \t\n" +
                      "key=value";

            var iniConfigSrc = new IniStreamConfigurationProvider(new IniStreamConfigurationSource());

            Assert.Throws(typeof(FormatException),() => iniConfigSrc.Load(StringToStream(ini)));
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
