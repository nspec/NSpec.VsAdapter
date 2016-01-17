using NSpec.Domain;
using NSpec.VsAdapter.Common;
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

            var contextFinder = new ContextFinder();

            var contexts = contextFinder.BuildContexts(assemblyPath);

            var examples = contexts.Examples();

            var debugInfoProvider = new DebugInfoProvider(assemblyPath, logger);

            var specMapper = new SpecMapper(assemblyPath, debugInfoProvider);

            var specifications = examples.Select(specMapper.FromExample);

            var specArray = specifications.ToArray();

            logger.Debug(String.Format("Finish collecting tests in '{0}'", assemblyPath));

            logger.Flush();

            return specArray;
        }

        readonly string assemblyPath;
        readonly ISerializableLogger logger;
    }
}
