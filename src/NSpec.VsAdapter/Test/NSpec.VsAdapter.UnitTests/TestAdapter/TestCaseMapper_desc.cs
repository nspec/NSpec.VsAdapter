using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Discovery;
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
        TestCaseMapper mapper;
        DiscoveredExample discoveredExample;
        TestCase expectedTestCase;

        AutoSubstitute autoSubstitute;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            mapper = autoSubstitute.Resolve<TestCaseMapper>();

            discoveredExample = new DiscoveredExample()
            {
                FullName = "nspec. L1 context. L2 context. example name.",
                SourceFilePath = @".\some\path\to\source\code.cs",
                SourceLineNumber = 123,
                SourceAssembly = @".\some\path\to\library.dll",
                Tags = new string[] 
                { 
                    "tag1", "tag2", "tag3_with_underscore", "tag4-with-dash", 
                },
            };

            expectedTestCase = new TestCase(
                discoveredExample.FullName,
                Constants.ExecutorUri,
                discoveredExample.SourceAssembly)
                {
                    DisplayName = "L1 context › L2 context › example name.",
                    CodeFilePath = discoveredExample.SourceFilePath,
                    LineNumber = discoveredExample.SourceLineNumber,
                };

            var expectedTraits = new string[] 
                { 
                    "tag1", "tag2", "tag3 with underscore", "tag4-with-dash", 
                }
                .Select(tag => new Trait(tag, null));

            expectedTestCase.Traits.AddRange(expectedTraits);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var testCase = mapper.FromDiscoveredExample(discoveredExample);

            testCase.ShouldBeEquivalentTo(expectedTestCase, options => 
                options.Excluding(tc => tc.Id));
        }

    }
}
