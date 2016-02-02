using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public class MultiSourceTestExecutorFactory : IMultiSourceTestExecutorFactory
    {
        public MultiSourceTestExecutorFactory(
            IBinaryTestExecutor binaryTestExecutor,
            IProgressRecorderFactory progressRecorderFactory,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestExecutor = binaryTestExecutor;
            this.progressRecorderFactory = progressRecorderFactory;
            this.loggerFactory = loggerFactory;
        }

        public IMultiSourceTestExecutor Create(IEnumerable<string> sources)
        {
            return new MultiSourceTestExecutor(sources, binaryTestExecutor, progressRecorderFactory, loggerFactory);
        }

        public IMultiSourceTestExecutor Create(IEnumerable<TestCase> testCases)
        {
            return new MultiSourceTestExecutor(testCases, binaryTestExecutor, progressRecorderFactory, loggerFactory);
        }

        readonly IBinaryTestExecutor binaryTestExecutor;
        readonly IProgressRecorderFactory progressRecorderFactory;
        readonly ILoggerFactory loggerFactory;
    }
}
