using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Settings;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NSpec.VsAdapter.UnitTests.Settings
{
    [TestFixture]
    [Category("AdapterSettingsProvider")]
    public abstract class AdapterSettingsProvider_desc_base
    {
        protected AdapterSettingsProvider provider;

        protected AutoSubstitute autoSubstitute;
        protected XmlSerializer serializer;
        protected XmlReader reader;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            serializer = Substitute.For<XmlSerializer>();
            autoSubstitute.Provide(serializer);

            provider = autoSubstitute.Resolve<AdapterSettingsProvider>();

            reader = Substitute.For<XmlReader>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            if (reader != null) reader.Dispose();
        }
    }

    public class AdapterSettingsProvider_when_created : AdapterSettingsProvider_desc_base
    {
        [Test]
        public void it_should_have_empty_settings()
        {
            provider.Load(reader);

            var expected = new AdapterSettings();
            var actual = provider.Settings;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public abstract class AdapterSettingProvider_when_succeeding_base : AdapterSettingsProvider_desc_base
    {
        protected readonly AdapterSettings someSettings = new AdapterSettings()
        {
            LogLevel = someLogLevel,
        };

        protected const string someLogLevel = "whatever value";

        public override void before_each()
        {
            base.before_each();

            reader.Read().Returns(true);

            reader.Name.Returns(AdapterSettings.RunSettingsXmlNode);

            serializer.Deserialize(reader).Returns(someSettings);
        }
    }

    public class AdapterSettingProvider_when_succeeding : AdapterSettingProvider_when_succeeding_base
    {
        [Test]
        public void it_should_fill_settings()
        {
            provider.Load(reader);

            var expected = someSettings;
            var actual = provider.Settings;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public abstract class AdapterSettingProvider_when_failing : AdapterSettingProvider_when_succeeding_base
    {
        public override void before_each()
        {
            base.before_each();

            // fill settings once, so to verify for empty settings later
            provider.Load(reader);
        }

        [Test]
        public void it_should_not_throw()
        {
            provider.Load(reader);
        }

        [Test]
        public void it_should_have_empty_settings()
        {
            provider.Load(reader);

            var expected = new AdapterSettings();
            var actual = provider.Settings;

            actual.ShouldBeEquivalentTo(expected, this.GetType().Name);
        }
    }

    public class AdapterSettingProvider_when_reader_is_null : AdapterSettingProvider_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            reader = null;
        }
    }

    public class AdapterSettingProvider_when_reader_returns_false : AdapterSettingProvider_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            reader.Read().Returns(false);
        }
    }

    public class AdapterSettingProvider_when_reader_fails : AdapterSettingProvider_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            reader.Read().Returns(_ => { throw new DummyTestException(this.GetType().Name); });
        }
    }

    public class AdapterSettingProvider_when_reader_name_does_not_match : AdapterSettingProvider_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            reader.Name.Returns(AdapterSettings.RunSettingsXmlNode + "Whatever");
        }
    }

    public class AdapterSettingProvider_when_deserialize_fails : AdapterSettingProvider_when_failing
    {
        public override void before_each()
        {
            base.before_each();

            serializer.Deserialize(reader).Returns(_ => { throw new DummyTestException(this.GetType().Name); });
        }
    }
}
