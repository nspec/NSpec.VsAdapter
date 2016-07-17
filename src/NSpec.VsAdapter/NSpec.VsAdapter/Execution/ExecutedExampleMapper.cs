using NSpec.Domain;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutedExampleMapper : IExecutedExampleMapper
    {
        public ExecutedExample FromExample(ExampleBase example)
        {
            var executed = new ExecutedExample()
            {
                FullName = example.FullName(),
                Pending = example.Pending,
                Failed = example.Failed(),
            };

            if (example.Exception != null)
            {
                executed.ExceptionMessage = example.Exception.Message;
                executed.ExceptionStackTrace = example.Exception.StackTrace;
            }

            return executed;
        }
    }
}
