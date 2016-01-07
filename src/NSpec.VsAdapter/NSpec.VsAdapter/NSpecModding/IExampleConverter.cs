using NSpec.Domain;
using System;
namespace NSpec.VsAdapter.NSpecModding
{
    public interface IExampleConverter
    {
        NSpecSpecification Convert(ExampleBase example);
    }
}
