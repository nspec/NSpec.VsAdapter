using NSpec.Domain;
using NSpec.VsAdapter.Common;
using System.Collections.Generic;

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
