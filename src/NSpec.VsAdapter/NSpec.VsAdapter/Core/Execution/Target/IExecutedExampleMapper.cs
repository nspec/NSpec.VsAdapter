using NSpec.Domain;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public interface IExecutedExampleMapper
    {
        ExecutedExample FromExample(ExampleBase example);
    }
}
