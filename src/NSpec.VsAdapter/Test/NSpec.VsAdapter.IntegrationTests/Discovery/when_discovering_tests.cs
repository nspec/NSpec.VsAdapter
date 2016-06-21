using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.IntegrationTests.TestData;
<<<<<<< HEAD
=======
using NSpec.VsAdapter.Settings;
using NSpec.VsAdapter.TestAdapter.Discovery;
using NUnit.Framework;
using System;
>>>>>>> origin/feat/unit-test-proxyables
using System.Collections.Generic;

namespace NSpec.VsAdapter.IntegrationTests.Discovery
{
    public class when_discovering_tests : when_discovering_tests_base
    {
        protected override string[] BuildSources()
        {
            return new string[]
            {
                TestConstants.SampleSpecsDllPath,
                TestConstants.SampleSystemDllPath,
            };
        }

        protected override IEnumerable<TestCase> BuildExpecteds()
        {
            return SampleSpecsTestCaseData.All;
        }
    }
}
