using System.IO;
using System.Reflection;

namespace NSpec.VsAdapter.IntegrationTests
{
    static class TestConstants
    {
        static TestConstants()
        {
            string thisProjectDllPath = Assembly.GetExecutingAssembly().Location;
            var dllFileInfo = new FileInfo(thisProjectDllPath);

#if DEBUG
            string configDir = "Debug";
#elif RELEASE
            string configDir = "Release";
#endif

            // move up from 'test\ProjectDir\Bin\<configuration>\' to 'test\'
            var testFolderInfo = dllFileInfo.Directory.Parent.Parent.Parent;

            TestFolderPath = TestUtils.FirstCharToUpper(testFolderInfo.FullName);

            SampleSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\test\SampleSpecs\desc_SystemUnderTest.cs");
            SampleSpecsDllPath = Path.Combine(new[]
            {
                TestFolderPath, @"Samples\test\SampleSpecs\bin", configDir, "SampleSpecs.dll"
            });
            SampleSystemDllPath = Path.Combine(new[]
            {
                TestFolderPath, @"Samples\src\SampleSystem\bin", configDir, "SampleSystem.dll"
            });

            ConfigSampleSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\test\ConfigSampleSpecs\desc_SystemWithSettings.cs");
            ConfigSampleSpecsDllPath = Path.Combine(new []
            {
                TestFolderPath, @"Samples\test\ConfigSampleSpecs\bin", configDir, "ConfigSampleSpecs.dll"
            });
            ConfigSampleSystemExePath = Path.Combine(new []
            {
                TestFolderPath, @"Samples\src\ConfigSampleSystem\bin", configDir, "ConfigSampleSystem.exe"
            });

            SampleAsyncSpecsSourcePath = Path.Combine(TestFolderPath, @"Samples\test\SampleAsyncSpecs\desc_AsyncSystemUnderTest.cs");
            SampleAsyncSpecsDllPath = Path.Combine(new []
            {
                TestFolderPath, @"Samples\test\SampleAsyncSpecs\bin", configDir, "SampleAsyncSpecs.dll"
            });
            SampleAsyncSystemDllPath = Path.Combine(new []
            {
                TestFolderPath, @"Samples\src\SampleAsyncSystem\bin", configDir, "SampleAsyncSystem.dll"
            });
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
