using NSpec.Domain;
using NSpec.VsAdapter.Common;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class CollectorInvocation : ICollectorInvocation
    {
        public CollectorInvocation(string binaryPath, ISerializableLogger logger)
        {
            this.binaryPath = binaryPath;
            this.logger = logger;
        }

        public DiscoveredExample[] Collect()
        {
            logger.Debug(String.Format("Start collecting tests in '{0}'", binaryPath));

            var exampleFinder = new ExampleFinder();

            var examples = exampleFinder.Find(binaryPath);

            var debugInfoProvider = new DebugInfoProvider(binaryPath, logger);

            var discoveredExampleMapper = new DiscoveredExampleMapper(binaryPath, debugInfoProvider);

            var discoveredExamples = examples.Select(discoveredExampleMapper.FromExample);

            var discoveredExampleArray = discoveredExamples.ToArray();

            logger.Debug(String.Format("Finish collecting tests in '{0}'", binaryPath));

            logger.Flush();

            return discoveredExampleArray;
        }

        readonly string binaryPath;
        readonly ISerializableLogger logger;
    }
}
