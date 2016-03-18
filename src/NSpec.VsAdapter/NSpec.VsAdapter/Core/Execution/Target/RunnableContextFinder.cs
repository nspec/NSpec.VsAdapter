using NSpec.Domain;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public class RunnableContextFinder
    {
        // Visual Studio test infrastructure requires a default constructor
        // Integration tests use this as well
        public RunnableContextFinder()
            : this(new ContextFinder())
        {
        }

        // Unit tests need a constructor with injected dependencies
        public RunnableContextFinder(IContextFinder contextFinder)
        {
            this.contextFinder = contextFinder;
        }

        public IEnumerable<RunnableContext> Find(string binaryPath, string[] exampleFullNames)
        {
            var contextCollection = contextFinder.BuildContextCollection(binaryPath);

            IEnumerable<RunnableContext> runnableContexts;

            if (exampleFullNames == RunAll)
            {
                IEnumerable<Context> allContexts = contextCollection;

                runnableContexts = allContexts.Select(ctx => new RunnableContext(ctx));
            }
            else
            {
                // original idea taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

                var allExamples = contextCollection.SelectMany(ctx => ctx.AllExamples());

                var selectedNames = new HashSet<string>(exampleFullNames);

                var selectedExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

                var selectedContexts = selectedExamples.Select(exm => exm.Context).Distinct();

                runnableContexts = selectedContexts.Select(ctx => new SelectedRunnableContext(ctx));
            }

            return runnableContexts;
        }

        public const string[] RunAll = null;

        readonly IContextFinder contextFinder;
    }
}
