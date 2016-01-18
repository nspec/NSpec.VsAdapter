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
        public CollectorInvocation(string binaryPath, ISerializableLogger logger)
        {
            this.binaryPath = binaryPath;
            this.logger = logger;
        }

        public NSpecSpecification[] Collect()
        {
            logger.Debug(String.Format("Start collecting tests in '{0}'", binaryPath));

            var exampleFinder = new ExampleFinder();

            var examples = exampleFinder.Find(binaryPath);

            var debugInfoProvider = new DebugInfoProvider(binaryPath, logger);

            var specMapper = new SpecMapper(binaryPath, debugInfoProvider);

            var specifications = examples.Select(specMapper.FromExample);

            var specArray = specifications.ToArray();

            logger.Debug(String.Format("Finish collecting tests in '{0}'", binaryPath));

            logger.Flush();

            return specArray;
        }

        readonly string binaryPath;
        readonly ISerializableLogger logger;
    }
}
