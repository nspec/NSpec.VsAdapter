using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Core.Execution;
using NSpec.VsAdapter.Core.Execution.Target;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.Execution.Target
{
    [TestFixture]
    [Category("ExecutedExampleMapper")]
    public class ExecutedExampleMapper_desc
    {
        ExecutedExampleMapper mapper;

        AutoSubstitute autoSubstitute;

        Context someContext;
        Action someAction;
        Uri someUri;
        const string tagText = "some-tag another-tag";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            someContext = new Context("some context");
            someAction = () => { };
            someUri = new Uri("http://www.example.com");

            mapper = autoSubstitute.Resolve<ExecutedExampleMapper>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_map_passed_example()
        {
            var someExample = new Example("some passed example", tagText, someAction, false)
            {
                Context = someContext,
                HasRun = true,
            };

            var expected = new ExecutedExample()
            {
                FullName = someExample.FullName(),
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void it_should_map_failed_example()
        {
            var someError = new DummyTestException();

            var someExample = new Example("some failed example", tagText, someAction, false)
            {
                Context = someContext,
                HasRun = true,
                Exception = someError,
            };

            var expected = new ExecutedExample()
            {
                FullName = someExample.FullName(),
                Failed = true,
                ExceptionMessage = someError.Message,
                ExceptionStackTrace = someError.StackTrace,
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void it_should_map_pending_example()
        {
            var someExample = new Example("some pending example", tagText, someAction, true)
            {
                Context = someContext,
                HasRun = false,
            };

            var expected = new ExecutedExample()
            {
                FullName = someExample.FullName(),
                Pending = true,
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
