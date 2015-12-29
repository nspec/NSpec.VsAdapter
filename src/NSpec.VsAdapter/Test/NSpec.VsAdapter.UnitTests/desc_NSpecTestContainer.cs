using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests
{
    [TestFixture]
    [Category("Container")]
    public abstract class base_desc_NSpecTestContainer
    {
        protected NSpecTestContainer container;

        protected AutoSubstitute autoSubstitute;
        protected ITestContainerDiscoverer containerDiscoverer;
        protected string sourcePath = "some/dummy/source";
        protected IEnumerable<Guid> debugEngines;
        protected IFileService fileService;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            containerDiscoverer = autoSubstitute.Resolve<ITestContainerDiscoverer>();

            debugEngines = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };

            fileService = autoSubstitute.Resolve<IFileService>();

            container = new NSpecTestContainer(containerDiscoverer, sourcePath, debugEngines, fileService);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    [TestFixture]
    [Category("Container")]
    public class when_creating_NSpecTestContainer : base_desc_NSpecTestContainer
    {
        [Test]
        public void it_should_return_container_discoverer()
        {
            container.Discoverer.Should().Be(containerDiscoverer);
        }

        [Test]
        public void it_should_return_source()
        {
            container.Source.Should().Be(sourcePath);
        }

        [Test]
        public void it_should_return_debug_engines()
        {
            container.DebugEngines.Should().Equal(debugEngines);
        }

        [Test]
        public void it_should_not_target_any_framework_version()
        {
            container.TargetFramework.Should().Be(FrameworkVersion.None);
        }

        [Test]
        public void it_should_target_any_cpu_platform()
        {
            container.TargetPlatform.Should().Be(Architecture.AnyCPU);
        }

        [Test]
        public void it_should_not_be_a_win8_metro_test_container()
        {
            container.IsAppContainerTestContainer.Should().BeFalse();

            container.DeployAppContainer().Should().BeNull();
        }
    }

    [TestFixture]
    [Category("Container")]
    public class when_comparing_NSpecTestContainer : base_desc_NSpecTestContainer
    {
        NSpecTestContainer other;
        string otherSourcePath;

        [SetUp]
        public override void before_each()
        {
            base.before_each();

            otherSourcePath = sourcePath + "/whatever";
        }

        [Test]
        public void it_should_not_match_when_other_is_null()
        {
            other = null;

            container.CompareTo(other).Should().Be(-1);
        }

        [Test]
        public void it_should_not_match_when_sources_are_different()
        {
            other = new NSpecTestContainer(containerDiscoverer, otherSourcePath, debugEngines, fileService);

            container.CompareTo(other).Should().Be(-1);
        }

        [Test]
        public void it_should_not_match_when_source_is_not_found()
        {
            other = new NSpecTestContainer(containerDiscoverer, otherSourcePath, debugEngines, fileService);

            fileService.Exists(sourcePath).Returns(false);

            container.CompareTo(other).Should().Be(-1);
        }

        [Test]
        public void it_should_not_match_when_timestamps_are_different()
        {
            other = new NSpecTestContainer(containerDiscoverer, otherSourcePath, debugEngines, fileService);

            var timestamp = DateTime.Now;
            var otherTimestamp = timestamp.AddHours(2);

            fileService.LastModified(sourcePath).Returns(timestamp);
            fileService.LastModified(otherSourcePath).Returns(otherTimestamp);

            container.CompareTo(other).Should().Be(-1);
        }

        [Test]
        public void it_should_match_when_sources_and_timestamps_are_the_same()
        {
            other = new NSpecTestContainer(containerDiscoverer, sourcePath, debugEngines, fileService);

            var timestamp = DateTime.Now;
            var otherTimestamp = timestamp;

            fileService.LastModified(sourcePath).Returns(timestamp);
            fileService.LastModified(otherSourcePath).Returns(otherTimestamp);

            container.CompareTo(other).Should().Be(0);
        }

        [Test]
        public void it_should_match_when_sources_have_different_case()
        {
            otherSourcePath = FlipCase(sourcePath);

            other = new NSpecTestContainer(containerDiscoverer, otherSourcePath, debugEngines, fileService);

            var timestamp = DateTime.Now;
            var otherTimestamp = timestamp;

            fileService.LastModified(sourcePath).Returns(timestamp);
            fileService.LastModified(otherSourcePath).Returns(otherTimestamp);

            container.CompareTo(other).Should().Be(0);
        }

        private static String FlipCase(String orig)
        {
            Func<char, String> selector =
                c => (char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c)).ToString();

            return orig.Select(selector).Aggregate(String.Concat);
        }
    }

    [TestFixture]
    [Category("Container")]
    public class when_cloning_NSpecTestContainer : base_desc_NSpecTestContainer
    {
        ITestContainer snapshot;

        [SetUp]
        public override void before_each()
        {
            base.before_each();

            snapshot = container.Snapshot();
        }

        [Test]
        public void it_should_return_same_properties()
        {
            snapshot.Discoverer.Should().Be(container.Discoverer);

            snapshot.Source.Should().Be(container.Source);

            snapshot.DebugEngines.Should().Equal(container.DebugEngines);

            snapshot.TargetFramework.Should().Be(container.TargetFramework);

            snapshot.TargetPlatform.Should().Be(container.TargetPlatform);

            snapshot.IsAppContainerTestContainer.Should().Be(container.IsAppContainerTestContainer);

            snapshot.DeployAppContainer().Should().Be(container.DeployAppContainer());
        }

        [Test]
        public void it_should_match_snapshot()
        {
            container.CompareTo(snapshot).Should().Be(0);
        }
    }
}
