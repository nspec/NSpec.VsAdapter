using FluentAssertions;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.UnitTests.CrossDomain;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery
{
    [Category("CrossDomainCollector")]
    public class CrossDomainCollector_when_run_succeeds 
        : CrossDomainRunner_when_run_succeeds<IEnumerable<DiscoveredExample>>
    {
        CrossDomainCollector collector;

        IEnumerable<DiscoveredExample> actualDiscoveredExamples;

        readonly static DiscoveredExample[] someDiscoveredExamples = new DiscoveredExample[] 
        { 
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        public override void before_each()
        {
            base.before_each();

            crossDomainProxy.Execute(targetOperation).Returns(someDiscoveredExamples);

            collector = autoSubstitute.Resolve<CrossDomainCollector>();

            actualDiscoveredExamples = collector.Run(somePath, targetOperation);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            actualDiscoveredExamples.Should().BeEquivalentTo(someDiscoveredExamples);
        }
    }

    [Category("CrossDomainCollector")]
    public class CrossDomainCollector_when_marshal_wrapper_creation_fails
        : CrossDomainRunner_when_marshal_wrapper_creation_fails<IEnumerable<DiscoveredExample>>
    {
        CrossDomainCollector collector;

        protected override void CreateRunner()
        {
            collector = autoSubstitute.Resolve<CrossDomainCollector>();
        }

        protected override void ExerciseRunner()
        {
            collector.Run(somePath, targetOperation);
        }
    }

    [Category("CrossDomainCollector")]
    public class CrossDomainCollector_when_marshaled_execution_fails
        : CrossDomainRunner_when_marshaled_execution_fails<IEnumerable<DiscoveredExample>>
    {
        CrossDomainCollector collector;

        protected override void CreateRunner()
        {
            collector = autoSubstitute.Resolve<CrossDomainCollector>();
        }

        protected override void ExerciseRunner()
        {
            collector.Run(somePath, targetOperation);
        }
    }
}
