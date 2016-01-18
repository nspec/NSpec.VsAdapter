using NSpec.Domain;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class RunnableContextFinder
    {
        // used by Visual Studio test infrastructure, by integration tests
        public RunnableContextFinder()
            : this(new ContextFinder())
        {
        }

        // used by unit tests
        public RunnableContextFinder(IContextFinder contextFinder)
        {
            this.contextFinder = contextFinder;
        }

        public IEnumerable<Context> Find(string binaryPath, string[] exampleFullNames)
        {
            var contextCollection = contextFinder.BuildContextCollection(binaryPath);

            if (exampleFullNames == RunAll)
            {
                return contextCollection.AllContexts();
            }

            // original idea taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

            var allExamples = contextCollection.SelectMany(ctx => ctx.Examples);

            var selectedNames = new HashSet<string>(exampleFullNames);

            var runnableExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

            var runnableContexts = runnableExamples.Select(exm => exm.Context).Distinct();

            return runnableContexts;
        }

        public const string[] RunAll = null;

        readonly IContextFinder contextFinder;
    }
}
