using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.TestData
{
    public class TestOutput
    {
        public string FullyQualifiedName { get; set; }

        public TestOutcome Outcome { get; set; }

        public string ErrorMessage { get; set; }

        public string Source { get; set; }

        public Uri ExecutorUri { get; set; }
    }
}
