using AutofacContrib.NSubstitute;
using NSpec.VsAdapter.Settings;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NSpec.VsAdapter.UnitTests.Settings
{
    [TestFixture]
    [Category("JsonSettingsRepository")]
    public abstract class JsonSettingsRepository_desc_base
    {
        protected JsonSettingsRepository repository;

        protected AutoSubstitute autoSubstitute;
        protected IFileService fileService;

        protected readonly string someJsonPath;
        protected const string someBinaryPath = @".\path\to\some\dummy-library.dll";

        public JsonSettingsRepository_desc_base()
        {
            someJsonPath = Path.Combine(@".\path\to\some\", JsonSettingsRepository.SettingsFileName);
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            fileService = autoSubstitute.Resolve<IFileService>();

            repository = autoSubstitute.Resolve<JsonSettingsRepository>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        protected void VerifyLogLevel(string propertyName, string propertyValue, string expected)
        {
            string someJsonTextContent = String.Format("{{ \"{0}\": \"{1}\" }}", propertyName, propertyValue);

            fileService.ReadAllText(someJsonPath).Returns(someJsonTextContent);

            repository.BinaryPath = someBinaryPath;

            repository.LogLevel.Should().Be(expected);
        }
    }

    public class JSonSettingsRepository_when_file_exists : JsonSettingsRepository_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(someJsonPath).Returns(true);
        }

        [Test]
        public void it_should_return_log_level_when_setting_exists()
        {
            string propertyName = "logLevel";
            string propertyValue = "some-value";
            string expected = "some-value";

            VerifyLogLevel(propertyName, propertyValue, expected);
        }

        [Test]
        public void it_should_return_default_log_level_when_setting_is_not_found()
        {
            string propertyName = "unknown prop";
            string propertyValue = "some-value";
            string expected = "info";

            VerifyLogLevel(propertyName, propertyValue, expected);
        }

        [Test]
        public void it_should_return_default_log_level_when_contents_are_not_json()
        {
            string someWrongTextContent = "123 this is not json $@#";
            string expected = "info";

            fileService.ReadAllText(someJsonPath).Returns(someWrongTextContent);

            repository.BinaryPath = someBinaryPath;
            
            repository.LogLevel.Should().Be(expected);
        }
    }

    public class JSonSettingsRepository_when_file_is_not_found : JsonSettingsRepository_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(someJsonPath).Returns(false);
        }

        [Test]
        public void it_should_return_default_log_level_when_setting_exists()
        {
            string propertyName = "logLevel";
            string propertyValue = "some-value";
            string expected = "info";

            VerifyLogLevel(propertyName, propertyValue, expected);
        }

        [Test]
        public void it_should_return_default_log_level_when_setting_is_not_found()
        {
            string propertyName = "unknown prop";
            string propertyValue = "some-value";
            string expected = "info";

            VerifyLogLevel(propertyName, propertyValue, expected);
        }
    }
}
