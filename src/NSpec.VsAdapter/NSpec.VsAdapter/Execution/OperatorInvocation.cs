using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class OperatorInvocation : IOperatorInvocation
    {
        public OperatorInvocation(string assemblyPath, IExecutionObserver executionObserver, ISerializableLogger logger)
        {
            this.assemblyPath = assemblyPath;
            this.executionObserver = executionObserver;
            this.logger = logger;
        }

        public int Operate()
        {
            logger.Debug(String.Format("Start operating tests in '{0}'", assemblyPath));

            var contexts = BuildContexts(assemblyPath);

            contexts.Run(executionObserver, false);

            int count = contexts.Count();

            logger.Debug(String.Format("Finish operating tests in '{0}'", assemblyPath));

            logger.Flush();

            return count;
        }

        static ContextCollection BuildContexts(string assemblyPath)
        {
            // TODO this is common to XxxInvocation: extract it, maybe in a base class or dependency

            var reflector = new Reflector(assemblyPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contexts = contextBuilder.Contexts();

            contexts.Build();

            return contexts;
        }

        readonly string assemblyPath;
        readonly IExecutionObserver executionObserver;
        readonly ISerializableLogger logger;
    }
}
