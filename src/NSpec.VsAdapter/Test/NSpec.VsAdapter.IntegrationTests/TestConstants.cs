using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    static class TestConstants
    {
        static TestConstants()
        {
            TestFolderPath = TestUtils.FirstCharToUpper(Path.GetFullPath(@"..\..\..\"));

            SampleSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\SampleSpecs\desc_SystemUnderTest.cs");

            SampleSpecsDllPath = Path.Combine(TestFolderPath, @"Samples\SampleSpecs\bin\Debug\SampleSpecs.dll");

            SampleSystemDllPath = Path.Combine(TestFolderPath, @"Samples\SampleSystem\bin\Debug\SampleSystem.dll");
        }

        public static readonly string TestFolderPath;

        public static readonly string SampleSpecsSourcePath;

        public static readonly string SampleSpecsDllPath;

        public static readonly string SampleSystemDllPath;
    }
}
