using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using NSpec.VsAdapter.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter.Execution
{
    public class MultiSourceTestExecutorFactory : IMultiSourceTestExecutorFactory
    {
        public MultiSourceTestExecutorFactory(
            IBinaryTestExecutor binaryTestExecutor,
            IProgressRecorderFactory progressRecorderFactory,
            ISettingsRepository settingsRepository,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestExecutor = binaryTestExecutor;
            this.progressRecorderFactory = progressRecorderFactory;
            this.settingsRepository = settingsRepository;
            this.loggerFactory = loggerFactory;
        }

        public IMultiSourceTestExecutor Create(IEnumerable<string> sources)
        {
            return new MultiSourceTestExecutor(sources, binaryTestExecutor, progressRecorderFactory, settingsRepository, loggerFactory);
        }

        public IMultiSourceTestExecutor Create(IEnumerable<TestCase> testCases)
        {
            return new MultiSourceTestExecutor(testCases, binaryTestExecutor, progressRecorderFactory, settingsRepository, loggerFactory);
        }

        readonly IBinaryTestExecutor binaryTestExecutor;
        readonly IProgressRecorderFactory progressRecorderFactory;
        readonly ISettingsRepository settingsRepository;
        readonly ILoggerFactory loggerFactory;
    }
}
