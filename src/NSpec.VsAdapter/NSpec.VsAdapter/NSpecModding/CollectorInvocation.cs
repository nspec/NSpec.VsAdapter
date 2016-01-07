using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public class CollectorInvocation : ICollectorInvocation
    {
        public CollectorInvocation(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public IEnumerable<NSpecSpecification> Collect()
        {
            var reflector = new Reflector(assemblyPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contexts = contextBuilder.Contexts();

            var builtContexts = contexts.Build();

            var examples = builtContexts.Examples();

            var exampleConverter = new ExampleConverter();

            var specifications = examples.Select(exampleConverter.Convert);

            return specifications;
        }

        readonly string assemblyPath;
    }
}
