using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.TestAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.TestAdapter
{
    [TestFixture]
    [Category("TestCaseMapper")]
    public class TestCaseMapper_desc
    {
        TestCaseMapper converter;
        NSpecSpecification specification;
        TestCase expectedTestCase;

        AutoSubstitute autoSubstitute;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            converter = autoSubstitute.Resolve<TestCaseMapper>();

            specification = new NSpecSpecification()
            {
                SourceFilePath = @".\some\path\to\library.dll",
                FullName = "some-test-full-name",
            };

            expectedTestCase = new TestCase(
                specification.FullName, 
                Constants.ExecutorUri, 
                specification.SourceFilePath);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var testCase = converter.FromSpecification(specification);

            testCase.Should().Match<TestCase>(tc =>
                tc.FullyQualifiedName == expectedTestCase.FullyQualifiedName &&
                tc.Source == expectedTestCase.Source &&
                tc.ExecutorUri == expectedTestCase.ExecutorUri);
        }

    }
}
