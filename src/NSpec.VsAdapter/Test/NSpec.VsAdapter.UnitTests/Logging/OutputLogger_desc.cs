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

        protected void VerifyMessageSent(LogMethod logMethod, TestMessageLevel messageLevel)
        {
            logMethod(someMessage);

            messageLogger.Received(1).SendMessage(messageLevel, Arg.Any<string>());
        }

        protected void VerifyMessageNotSent(LogMethod logMethod)
        {
            logMethod(someMessage);

            messageLogger.DidNotReceive().SendMessage(Arg.Any<TestMessageLevel>(), Arg.Any<string>());
        }

        protected delegate void LogMethod(string message);
    }

    public class OutputLogger_when_min_level_is_debug : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("Debug");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_report_debug()
        {
            VerifyMessageSent(logger.Debug, TestMessageLevel.Informational);
        }

        [Test]
        public void it_should_report_info()
        {
            VerifyMessageSent(logger.Info, TestMessageLevel.Informational);
        }

        [Test]
        public void it_should_report_warning()
        {
            VerifyMessageSent(logger.Warn, TestMessageLevel.Warning);
        }

        [Test]
        public void it_should_report_error()
        {
            VerifyMessageSent(logger.Error, TestMessageLevel.Error);
        }
    }

    public class OutputLogger_when_min_level_is_info : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("Info");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            VerifyMessageNotSent(logger.Debug);
        }

        [Test]
        public void it_should_report_info()
        {
            VerifyMessageSent(logger.Info, TestMessageLevel.Informational);
        }

        [Test]
        public void it_should_report_warning()
        {
            VerifyMessageSent(logger.Warn, TestMessageLevel.Warning);
        }

        [Test]
        public void it_should_report_error()
        {
            VerifyMessageSent(logger.Error, TestMessageLevel.Error);
        }
    }

    public class OutputLogger_when_min_level_is_warning : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("Warning");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            VerifyMessageNotSent(logger.Debug);
        }

        [Test]
        public void it_should_not_report_info()
        {
            VerifyMessageNotSent(logger.Info);
        }

        [Test]
        public void it_should_report_warning()
        {
            VerifyMessageSent(logger.Warn, TestMessageLevel.Warning);
        }

        [Test]
        public void it_should_report_error()
        {
            VerifyMessageSent(logger.Error, TestMessageLevel.Error);
        }
    }

    public class OutputLogger_when_min_level_is_error : OutputLogger_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            settingsRepo.LogLevel.Returns("Error");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }

        [Test]
        public void it_should_not_report_debug()
        {
            VerifyMessageNotSent(logger.Debug);
        }

        [Test]
        public void it_should_not_report_info()
        {
            VerifyMessageNotSent(logger.Info);
        }

        [Test]
        public void it_should_not_report_warning()
        {
            VerifyMessageNotSent(logger.Warn);
        }

        [Test]
        public void it_should_report_error()
        {
            VerifyMessageSent(logger.Error, TestMessageLevel.Error);
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

            settingsRepo.LogLevel.Returns("Unknown");

            logger = autoSubstitute.Resolve<OutputLogger>();
        }
    }
}
