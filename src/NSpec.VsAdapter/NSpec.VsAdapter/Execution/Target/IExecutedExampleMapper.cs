using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution.Target
{
    public interface IExecutedExampleMapper
    {
        ExecutedExample FromExample(ExampleBase example);
    }
}
