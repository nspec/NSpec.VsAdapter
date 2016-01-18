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
        : CrossDomainRunner_when_run_succeeds<IEnumerable<NSpecSpecification>>
    {
        CrossDomainCollector collector;

        IEnumerable<NSpecSpecification> actualSpecifications;

        readonly static NSpecSpecification[] someSpecifications = new NSpecSpecification[] 
        { 
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        public override void before_each()
        {
            base.before_each();

            crossDomainProxy.Execute(targetOperation).Returns(someSpecifications);

            collector = autoSubstitute.Resolve<CrossDomainCollector>();

            actualSpecifications = collector.Run(somePath, targetOperation);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            actualSpecifications.Should().BeEquivalentTo(someSpecifications);
        }
    }

    [Category("CrossDomainCollector")]
    public class CrossDomainCollector_when_marshal_wrapper_creation_fails
        : CrossDomainRunner_when_marshal_wrapper_creation_fails<IEnumerable<NSpecSpecification>>
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
        : CrossDomainRunner_when_marshaled_execution_fails<IEnumerable<NSpecSpecification>>
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
