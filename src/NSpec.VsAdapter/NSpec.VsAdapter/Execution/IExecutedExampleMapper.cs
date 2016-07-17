using NSpec.Domain;

namespace NSpec.VsAdapter.Execution
{
    public interface IExecutedExampleMapper
    {
        ExecutedExample FromExample(ExampleBase example);
    }
}
