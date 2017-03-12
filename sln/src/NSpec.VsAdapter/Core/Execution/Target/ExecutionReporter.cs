using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace NSpec.VsAdapter.Core.Execution.Target
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
