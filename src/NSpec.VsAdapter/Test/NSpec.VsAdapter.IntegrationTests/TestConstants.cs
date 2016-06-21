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

            ConfigSampleSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\ConfigSampleSpecs\desc_SystemWithSettings.cs");
            ConfigSampleSpecsDllPath = Path.Combine(TestFolderPath, @"Samples\ConfigSampleSpecs\bin\Debug\ConfigSampleSpecs.dll");

            ConfigSampleSystemExePath = Path.Combine(TestFolderPath, @"Samples\ConfigSampleSystem\bin\Debug\ConfigSampleSystem.exe");

            SampleAsyncSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\SampleAsyncSpecs\desc_AsyncSystemUnderTest.cs");
            SampleAsyncSpecsDllPath = Path.Combine(TestFolderPath, @"Samples\SampleAsyncSpecs\bin\Debug\SampleAsyncSpecs.dll");

            SampleAsyncSystemDllPath = Path.Combine(TestFolderPath, @"Samples\SampleAsyncSystem\bin\Debug\SampleAsyncSystem.dll");
        }

        public static readonly string TestFolderPath;

        // Sample System & Specs

        public static readonly string SampleSpecsSourcePath;
        public static readonly string SampleSpecsDllPath;

        public static readonly string SampleSystemDllPath;

        // Config Sample System & Specs

        public static readonly string ConfigSampleSpecsDllPath;
        public static readonly string ConfigSampleSpecsSourcePath;

        public static readonly string ConfigSampleSystemExePath;

        // Sample Async System & Specs

        public static readonly string SampleAsyncSpecsSourcePath;
        public static readonly string SampleAsyncSpecsDllPath;

        public static readonly string SampleAsyncSystemDllPath;
    }
}
