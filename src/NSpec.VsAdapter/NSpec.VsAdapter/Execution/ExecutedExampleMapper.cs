using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Exception = example.Exception,
            };

            return executed;
        }
    }
}
