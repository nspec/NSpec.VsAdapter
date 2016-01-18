using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Common
{
    public class ContextFinder : IContextFinder
    {
        public ContextCollection BuildContextCollection(string binaryPath)
        {
            var reflector = new Reflector(binaryPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contexts = contextBuilder.Contexts();

            contexts.Build();

            return contexts;
        }
    }
}
