using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests
{
    static class TestConstants
    {
        public static readonly string SolutionTestFolderPath = 
            Path.GetFullPath(@"..\..\..\");

        public static readonly string SampleSpecsDllPath =
            Path.Combine(SolutionTestFolderPath, @"SampleSpecs\bin\Debug\SampleSpecs.dll");

        public static readonly string SampleSystemDllPath =
            Path.Combine(SolutionTestFolderPath, @"SampleSystem\bin\Debug\SampleSystem.dll");
    }
}
