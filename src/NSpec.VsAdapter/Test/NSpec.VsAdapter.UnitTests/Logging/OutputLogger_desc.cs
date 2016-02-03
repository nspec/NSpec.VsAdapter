using AutofacContrib.NSubstitute;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Logging;
using NSpec.VsAdapter.Settings;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Logging
{
    [TestFixture]
    [Category("OutputLogger")]
    public abstract class OutputLogger_desc_base
    {
        protected OutputLogger logger;

        protected AutoSubstitute autoSubstitute;
        protected IMessageLogger messageLogger;
        protected ISettingsRepository settingsRepo;

        protected const string someMessage = "whatever";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            messageLogger = autoSubstitute.Resolve<IMessageLogger>();

            settingsRepo = autoSubstitute.Resolve<ISettingsRepository>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class OutputLogger_when_min_level_is_debug : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("debug");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_report_debug()
        {
            logger.Debug(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Informational, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_info()
        {
            logger.Info(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Informational, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_warning()
        {
            logger.Warn(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Warning, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_error()
        {
            logger.Error(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Error, Arg.Any<string>());
        }
    }

    public class OutputLogger_when_min_level_is_info : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("info");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            logger.Debug(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_report_info()
        {
            logger.Info(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Informational, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_warning()
        {
            logger.Warn(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Warning, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_error()
        {
            logger.Error(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Error, Arg.Any<string>());
        }
    }

    public class OutputLogger_when_min_level_is_warning : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("warning");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            logger.Debug(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_not_report_info()
        {
            logger.Info(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_report_warning()
        {
            logger.Warn(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Warning, Arg.Any<string>());
        }

        [Test]
        public void it_should_report_error()
        {
            logger.Error(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Error, Arg.Any<string>());
        }
    }

    public class OutputLogger_when_min_level_is_error : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("error");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            logger.Debug(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_not_report_info()
        {
            logger.Info(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_not_report_warning()
        {
            logger.Warn(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        [Test]
        public void it_should_report_error()
        {
            logger.Error(someMessage);

            messageLogger.Received(1).SendMessage(TestMessageLevel.Error, Arg.Any<string>());
        }
    }

    public class OutputLogger_when_min_level_is_null : OutputLogger_when_min_level_is_info
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns((string)null);

            logger = autoSubstitute.Resolve<OutputLogger>();
        }
    }

    public class OutputLogger_when_min_level_is_empty : OutputLogger_when_min_level_is_info
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns(String.Empty);

            logger = autoSubstitute.Resolve<OutputLogger>();
        }
    }

    public class OutputLogger_when_min_level_is_unknown : OutputLogger_when_min_level_is_info
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("unknown");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }
    }
}
