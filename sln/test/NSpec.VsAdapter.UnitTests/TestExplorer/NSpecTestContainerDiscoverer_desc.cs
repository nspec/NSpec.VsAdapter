﻿using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using Microsoft.VisualStudio.TestWindow.Extensibility.Model;
using NSpec.VsAdapter.ProjectObservation;
using NSpec.VsAdapter.TestExplorer;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.TestExplorer
{
    [TestFixture]
    [Category("NSpecTestContainerDiscoverer")]
    public abstract class NSpecTestContainerDiscoverer_desc_base
    {
        protected NSpecTestContainerDiscoverer containerDiscoverer;

        protected AutoSubstitute autoSubstitute;
        protected Subject<IEnumerable<string>> testDllPathStream;
        protected ITestContainerFactory containerFactory;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var testBinaryNotifier = autoSubstitute.Resolve<ITestBinaryNotifier>();
            testDllPathStream = new Subject<IEnumerable<string>>();
            testBinaryNotifier.PathStream.Returns(testDllPathStream);

            containerFactory = autoSubstitute.Resolve<ITestContainerFactory>();

            containerDiscoverer = new NSpecTestContainerDiscoverer(testBinaryNotifier, containerFactory);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            testDllPathStream.Dispose();
            containerDiscoverer.Dispose();
        }
    }

    public class NSpecTestContainerDiscoverer_when_notified : NSpecTestContainerDiscoverer_desc_base
    {
        int updatedEventNr;

        string[] dllPaths = new string[]
        {
            @".\some\dummy\library.dll",
            @".\another\dummy\library.dll",
        };

        public override void before_each()
        {
            base.before_each();

            updatedEventNr = 0;

            containerDiscoverer.TestContainersUpdated += (s, e) =>
                {
                    updatedEventNr++;
                };

            containerFactory.Create(null, null).ReturnsForAnyArgs(callInfo =>
                {
                    var discoverer = callInfo.Arg<ITestContainerDiscoverer>();
                    var path = callInfo.Arg<string>();

                    var container = new DummyTestContainer();

                    container.Discoverer = discoverer;
                    container.Source = path;

                    return container;
                });
        }

        [Test]
        public void it_should_return_executor_uri()
        {
            containerDiscoverer.ExecutorUri.Should().Be(Constants.ExecutorUriString);
        }

        [Test]
        public void it_should_return_no_containers_when_created()
        {
            containerDiscoverer.TestContainers.Should().BeEmpty();
        }

        [Test]
        public void it_should_return_one_container_for_each_notified_dll()
        {
            testDllPathStream.OnNext(dllPaths);

            containerDiscoverer.TestContainers.Should().HaveCount(2);
        }

        [Test]
        public void it_should_return_containers_with_dll_paths()
        {
            testDllPathStream.OnNext(dllPaths);

            containerDiscoverer.TestContainers.Select(c => c.Source).Should().Equal(dllPaths);
        }

        [Test]
        public void it_should_return_containers_with_this_container_discoverer()
        {
            testDllPathStream.OnNext(dllPaths);

            containerDiscoverer.TestContainers.All(c => c.Discoverer == containerDiscoverer).Should().BeTrue();
        }

        [Test]
        public void it_should_not_raise_event_when_created()
        {
            updatedEventNr.Should().Be(0);
        }

        [Test]
        public void it_should_raise_event_when_notified_with_dll_paths()
        {
            testDllPathStream.OnNext(dllPaths);

            updatedEventNr.Should().Be(1);
        }

        [Test]
        public void it_should_raise_event_when_notified_with_no_dll_paths()
        {
            var noDllPaths = new string[] { };

            testDllPathStream.OnNext(noDllPaths);

            updatedEventNr.Should().Be(1);
        }

        [Test]
        public void it_should_raise_event_twice_when_notified_twice()
        {
            testDllPathStream.OnNext(dllPaths);
            testDllPathStream.OnNext(dllPaths);

            updatedEventNr.Should().Be(2);
        }

        class DummyTestContainer : ITestContainer
        {
            public int CompareTo(ITestContainer other)
            {
                throw new DummyTestException();
            }

            public IEnumerable<Guid> DebugEngines
            {
                get { throw new DummyTestException(); }
            }

            public IDeploymentData DeployAppContainer()
            {
                throw new DummyTestException();
            }

            public ITestContainerDiscoverer Discoverer { get; set; }

            public bool IsAppContainerTestContainer
            {
                get { throw new DummyTestException(); }
            }

            public ITestContainer Snapshot()
            {
                throw new DummyTestException();
            }

            public string Source { get; set; }

            public FrameworkVersion TargetFramework
            {
                get { throw new DummyTestException(); }
            }

            public Architecture TargetPlatform
            {
                get { throw new DummyTestException(); }
            }
        }
    }
}
