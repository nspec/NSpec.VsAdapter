using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery.SampleSpecs
{
    public static class SampleTestCaseDebugInfo
    {
        public readonly static
            Dictionary<string, Dictionary<string, DiaNavigationData>> ByClassMethodName;

        public readonly static IEnumerable<DiaNavigationData> All;

        static SampleTestCaseDebugInfo()
        {
            string sourceCodeFilePath = SpecSampleFileInfo.ThisSourceCodeFilePath;

            ByClassMethodName = new Dictionary<string, Dictionary<string, DiaNavigationData>>()
            {
                { 
                    "NSpec.VsAdapter.UnitTests.Discovery.SampleSpecs.ParentSpec", 
                    new Dictionary<string,DiaNavigationData>()
                    {
                        {
                            "method_context_1",
                            new DiaNavigationData(sourceCodeFilePath, 18, 22)
                        },
                        {
                            "method_context_2",
                            new DiaNavigationData(sourceCodeFilePath, 25, 27)
                        },
                    }
                },
                { 
                    "NSpec.VsAdapter.UnitTests.Discovery.SampleSpecs.ChildSpec", 
                    new Dictionary<string,DiaNavigationData>()
                    {
                        {
                            "method_context_3",
                            new DiaNavigationData(sourceCodeFilePath, 35, 37)
                        },
                        {
                            "method_context_4",
                            new DiaNavigationData(sourceCodeFilePath, 40, 42)
                        },
                    }
                },
            };

            All = ByClassMethodName
                .SelectMany(byClassName => byClassName.Value)
                .Select(byMethodName => byMethodName.Value);
        }
    }
}
