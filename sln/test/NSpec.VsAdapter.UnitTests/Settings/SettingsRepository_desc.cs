using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.Settings;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Settings
{
    [TestFixture]
    [Category("SettingsRepository")]
    public abstract class SettingsRepository_desc_base
    {
        protected SettingsRepository repository;

        protected AutoSubstitute autoSubstitute;
        protected IDiscoveryContext discoveryContext;
        protected IRunSettings runSettings;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            repository = autoSubstitute.Resolve<SettingsRepository>();

            discoveryContext = autoSubstitute.Resolve<IDiscoveryContext>();
            runSettings = autoSubstitute.Resolve<IRunSettings>();
            
            discoveryContext.RunSettings.Returns(runSettings);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class SettingsRepository_when_succeeding : SettingsRepository_desc_base
    {
        readonly AdapterSettings someSettings = new AdapterSettings()
        {
            LogLevel = someLogLevel,
        };

        const string someLogLevel = "whatever value";

        public override void before_each()
        {
            base.before_each();

            var settingsProvider = autoSubstitute.Resolve<IAdapterSettingsProvider>();

            runSettings.GetSettings(AdapterSettings.RunSettingsXmlNode).Returns(settingsProvider);

            settingsProvider.Settings.Returns(someSettings);
        }

        [Test]
        public void it_should_pass_provider_settings()
        {
            var expected = someSettings;
            var actual = repository.Load(discoveryContext);

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public abstract class SettingsRepository_when_failing : SettingsRepository_desc_base
    {
        [Test]
        public void it_should_return_empty_settings()
        {
            var expected = new AdapterSettings();
            var actual = repository.Load(discoveryContext);

            actual.ShouldBeEquivalentTo(expected, this.GetType().Name);
        }
    }

    public class SettingsRepository_when_getsettings_fails : SettingsRepository_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            runSettings.GetSettings(Arg.Any<string>()).Returns(_ => { throw new DummyTestException(); });
        }
    }

    public class SettingsRepository_when_settings_provider_has_wrong_type : SettingsRepository_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            var wrongProvider = Substitute.For<ISettingsProvider>();

            runSettings.GetSettings(Arg.Any<string>()).Returns(wrongProvider);
        }
    }
}
