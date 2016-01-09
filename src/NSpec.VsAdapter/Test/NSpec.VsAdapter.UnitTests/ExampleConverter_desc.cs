using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
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
    [Category("ExampleConverter")]
    public class ExampleConverter_desc
    {
        ExampleConverter converter;

        AutoSubstitute autoSubstitute;
        IDebugInfoProvider debugInfoProvider;
        Example example;
        NSpecSpecification expectedSpec;

        const string someAssemblyPath = @".\some\path\to\assembly.dll";
        const string someSourceCodePath = @".\some\path\to\source\code.cs";
        const int someLineNumber = 123;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var parentContext = new Context("some parent context");

            var context = new Context("some child context");
            context.Parent = parentContext;

            Action someAction = () => { };

            example = new Example(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                someAction);

            string exampleClassName = typeof(Example).FullName;
            string exampleMethodName = someAction.Method.Name;

            example.Context = context;

            example.Spec = "some specification";

            expectedSpec = new NSpecSpecification()
            {
                FullName = example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = someSourceCodePath,
                SourceLineNumber = someLineNumber,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            var navigationData = new DiaNavigationData(someSourceCodePath, someLineNumber, someLineNumber + 4);

            debugInfoProvider = autoSubstitute.Resolve<IDebugInfoProvider>();
            debugInfoProvider.GetNavigationData(exampleClassName, exampleMethodName).Returns(navigationData);

            converter = new ExampleConverter(someAssemblyPath, debugInfoProvider);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var spec = converter.Convert(example);

            spec.ShouldBeEquivalentTo(expectedSpec);
        }
    }
}
