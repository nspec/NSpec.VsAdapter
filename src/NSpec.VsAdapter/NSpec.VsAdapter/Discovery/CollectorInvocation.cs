using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class CollectorInvocation : ICollectorInvocation
    {
        public CollectorInvocation(string assemblyPath, ISerializableLogger logger)
        {
            this.assemblyPath = assemblyPath;
            this.logger = logger;
        }

        public NSpecSpecification[] Collect()
        {
            logger.Debug(String.Format("Start collecting tests in '{0}'", assemblyPath));

            var contexts = FindContexts(assemblyPath);

            var builtContexts = contexts.Build();

            var examples = builtContexts.Examples();

            var debugInfoProvider = new DebugInfoProvider(assemblyPath, logger);

            var specMapper = new SpecMapper(assemblyPath, debugInfoProvider);

            var specifications = examples.Select(specMapper.FromExample);

            logger.Debug(String.Format("Finish collecting tests in '{0}'", assemblyPath));

            var specArray = specifications.ToArray();

            logger.Flush();

            return specArray;
        }

        static ContextCollection FindContexts(string assemblyPath)
        {
            var reflector = new Reflector(assemblyPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contexts = contextBuilder.Contexts();

            return contexts;
        }

        readonly string assemblyPath;
        readonly ISerializableLogger logger;
    }
}
