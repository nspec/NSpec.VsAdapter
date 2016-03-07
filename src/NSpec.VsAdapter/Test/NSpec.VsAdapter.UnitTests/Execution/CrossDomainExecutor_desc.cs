using FluentAssertions;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.UnitTests.CrossDomain;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO rimuovere o adeguare al nuovo
#if false
namespace NSpec.VsAdapter.UnitTests.Execution
{
    [Category("CrossDomainExecutor")]
    public class CrossDomainExecutor_when_run_succeeds
        : CrossDomainRunner_when_run_succeeds<int>
    {
        CrossDomainExecutor executor;

        int expectedCount = 17;
        int actualCount;

        public override void before_each()
        {
            base.before_each();

            crossDomainProxy.Execute(targetOperation).Returns(expectedCount);

            executor = autoSubstitute.Resolve<CrossDomainExecutor>();

            actualCount = executor.Run(somePath, targetOperation);
        }

        [Test]
        public void it_should_execute_tests()
        {
            actualCount.Should().Be(expectedCount);
        }
    }

    [Category("CrossDomainExecutor")]
    public class CrossDomainExecutor_when_marshal_wrapper_creation_fails
        : CrossDomainRunner_when_marshal_wrapper_creation_fails<int>
    {
        CrossDomainExecutor executor;

        protected override void CreateRunner()
        {
            executor = autoSubstitute.Resolve<CrossDomainExecutor>();
        }

        protected override void ExerciseRunner()
        {
            executor.Run(somePath, targetOperation);
        }
    }

    [Category("CrossDomainExecutor")]
    public class CrossDomainExecutor_when_marshaled_execution_fails
        : CrossDomainRunner_when_marshaled_execution_fails<int>
    {
        CrossDomainExecutor executor;

        protected override void CreateRunner()
        {
            executor = autoSubstitute.Resolve<CrossDomainExecutor>();
        }

        protected override void ExerciseRunner()
        {
            executor.Run(somePath, targetOperation);
        }
    }
}
#endif
