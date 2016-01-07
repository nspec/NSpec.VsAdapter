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
                FullName = "some-test-full-name",
                SourceFilePath = @".\some\path\to\source\code.cs",
                SourceLineNumber = 123,
                SourceAssembly = @".\some\path\to\library.dll",
            };

            expectedTestCase = new TestCase(
                specification.FullName,
                Constants.ExecutorUri,
                specification.SourceAssembly)
                {
                    DisplayName = specification.FullName,
                    CodeFilePath = specification.SourceFilePath,
                    LineNumber = specification.SourceLineNumber,
                };
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

            testCase.ShouldBeEquivalentTo(expectedTestCase, options => 
                options.Excluding(tc => tc.Id));
        }

    }
}
