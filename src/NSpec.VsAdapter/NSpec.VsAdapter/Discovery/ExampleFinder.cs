using NSpec.Domain;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class ExampleFinder
    {
        public IEnumerable<ExampleBase> Find(string binaryPath)
        {
            var contextFinder = new ContextFinder();

            var contexts = contextFinder.BuildContextCollection(binaryPath);

            var examples = contexts.Examples();

            return examples;
        }
    }
}
