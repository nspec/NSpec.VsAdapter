using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.Discovery.SampleSpecs
{
    public static class SampleDebugInfo
    {
        public readonly static
            Dictionary<string, Dictionary<string, Dictionary<string, DiaNavigationData>>> ByClassMethodActionName;

        public readonly static
            Dictionary<string, Dictionary<string, DiaNavigationData>> ByClassActionName;

        public readonly static IEnumerable<DiaNavigationData> All;

        static SampleDebugInfo()
        {
            string sourceCodeFilePath = SpecSampleFileInfo.ThisSourceCodeFilePath;

            ByClassMethodActionName = new Dictionary<string, Dictionary<string, Dictionary<string, DiaNavigationData>>>()
            {
                { 
                    "NSpec.VsAdapter.UnitTests.Core.Discovery.SampleSpecs.ParentSpec", 
                    new Dictionary<string, Dictionary<string, DiaNavigationData>>()
                    {
                        {
                            "method_context_1",
                            new Dictionary<string, DiaNavigationData>() 
                            {
                                {
                                    "<method_context_1>b__0",
                                    new DiaNavigationData(sourceCodeFilePath, 19, 19)
                                },
                                {
                                    "<method_context_1>b__1",
                                    new DiaNavigationData(sourceCodeFilePath, 21, 21)
                                },
                            }
                        },
                        {
                            "method_context_2",
                            new Dictionary<string, DiaNavigationData>() 
                            {
                                {
                                    "<method_context_2>b__2",
                                    new DiaNavigationData(sourceCodeFilePath, 26, 26)
                                },
                            }
                        },
                    }
                },
                { 
                    "NSpec.VsAdapter.UnitTests.Core.Discovery.SampleSpecs.ChildSpec", 
                    new Dictionary<string, Dictionary<string, DiaNavigationData>>()
                    {
                        {
                            "method_context_3",
                            new Dictionary<string, DiaNavigationData>() 
                            {
                                {
                                    "<method_context_3>b__0",
                                    // no source code info available for pending tests
                                    new DiaNavigationData(String.Empty, 0, 0)
                                },
                            }
                        },
                        {
                            "method_context_4",
                            new Dictionary<string, DiaNavigationData>() 
                            {
                                {
                                    "<method_context_4>b__0",
                                    new DiaNavigationData(sourceCodeFilePath, 41, 41)
                                },
                            }
                        },
                    }
                },
            };

            All = ByClassMethodActionName
                .SelectMany(byClassName => byClassName.Value)
                .SelectMany(byMethodName => byMethodName.Value)
                .Select(byActionName => byActionName.Value);

            ByClassActionName = new Dictionary<string, Dictionary<string, DiaNavigationData>>();

            ByClassMethodActionName.Keys.Do(className =>
                {
                    var methodInfos = ByClassMethodActionName[className];

                    var actionInfos = methodInfos
                        .SelectMany(methodInfo => methodInfo.Value)
                        .ToDictionary(actionInfo => actionInfo.Key, actionInfo => actionInfo.Value);

                    ByClassActionName[className] = actionInfos;
                });
        }
    }
}
