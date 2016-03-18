using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution.Target
{
    public class ExecutionReporter : ILiveFormatter
    {
        public ExecutionReporter(IProgressRecorder progressRecorder, IExecutedExampleMapper executedExampleMapper)
        {
            this.progressRecorder = progressRecorder;
            this.executedExampleMapper = executedExampleMapper;
        }

        public void Write(ExampleBase example, int level)
        {
            // ignore level

            var executedExample = executedExampleMapper.FromExample(example);

            progressRecorder.RecordExecutedExample(executedExample);
        }

        public void Write(Context context)
        {
            // do nothing
        }

        readonly IProgressRecorder progressRecorder;
        readonly IExecutedExampleMapper executedExampleMapper;
    }
}
