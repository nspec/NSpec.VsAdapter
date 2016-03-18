using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.TestAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution
{
    public class ProgressRecorderFactory : IProgressRecorderFactory
    {
        public ProgressRecorderFactory(ITestResultMapper testResultMapper)
        {
            this.testResultMapper = testResultMapper;
        }

        public IProgressRecorder Create(ITestExecutionRecorder testExecutionRecorder)
        {
            return new ProgressRecorder(testExecutionRecorder, testResultMapper);
        }

        readonly ITestResultMapper testResultMapper;
    }
}
