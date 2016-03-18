using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public class ExecutionReporterFactory : IExecutionReporterFactory
    {
        public ExecutionReporterFactory()
        {
            executedExampleMapper = new ExecutedExampleMapper();
        }

        public ILiveFormatter Create(IProgressRecorder progressRecorder)
        {
            return new ExecutionReporter(progressRecorder, executedExampleMapper);
        }

        readonly IExecutedExampleMapper executedExampleMapper;
    }
}
