using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    public class ExampleConverter
    {
        public ExampleConverter(string assemblyPath, IDebugInfoProvider debugInfoProvider)
        {
            this.assemblyPath = assemblyPath;
            this.debugInfoProvider = debugInfoProvider;
        }

        public NSpecSpecification Convert(ExampleBase example)
        {
            var methodInfo = ReflectExampleMethod(example);

            string specClassName = methodInfo.DeclaringType.FullName;
            string exampleMethodName = methodInfo.Name;

            var navigationData = debugInfoProvider.GetNavigationData(specClassName, exampleMethodName);

            var specification = new NSpecSpecification()
            {
                FullName = example.FullName(),
                SourceAssembly = assemblyPath,
                SourceFilePath = navigationData.FileName,
                SourceLineNumber = navigationData.MinLineNumber,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            return specification;
        }

        readonly string assemblyPath;
        readonly IDebugInfoProvider debugInfoProvider;

        // taken from https://github.com/BrainCrumbz/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Discoverer.cs

        static MethodInfo ReflectExampleMethod(ExampleBase example)
        {
            const string privateMethodFieldName = "method";
            const string privateActionFieldName = "action";

            MethodInfo info;

            if (example is MethodExample)
            {
                info = example.GetType()
                    .GetField(privateMethodFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(example) as MethodInfo;
            }
            else
            {
                var action = example.GetType()
                    .GetField(privateActionFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(example) as Action;

                info = action.Method;
            }

            return info;
        }
    }
}
