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
        public IEnumerable<Context> Find(string binaryPath, string[] exampleFullNames)
        {
            var contextFinder = new ContextFinder();

            var contexts = contextFinder.BuildContexts(binaryPath);

            if (exampleFullNames == RunAll)
            {
                return contexts.AllContexts();
            }

            // original idea taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

            var allExamples = contexts.SelectMany(ctx => ctx.Examples);

            var selectedNames = new HashSet<string>(exampleFullNames);

            var runnableExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

            var runnableContexts = runnableExamples.Select(exm => exm.Context);

            return runnableContexts;
        }

        public const string[] RunAll = null;
    }
}
