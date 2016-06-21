using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class DiscoveredExampleMapper
    {
        public DiscoveredExampleMapper(string binaryPath, IDebugInfoProvider debugInfoProvider)
        {
            typeNameToBodyGetterMap = new Dictionary<string, BaseExampleBodyGetter>()
            {
                {
                    typeof(Example).Name,
                    GetExampleBodyInfo
                },
                {
                    typeof(MethodExample).Name,
                    GetMethodExampleBodyInfo
                },
                {
                    typeof(AsyncExample).Name,
                    GetAsyncExampleBodyInfo
                },
                {
                    typeof(AsyncMethodExample).Name,
                    GetAsyncMethodExampleBodyInfo
                },
            };

            this.binaryPath = binaryPath;
            this.debugInfoProvider = debugInfoProvider;
        }

        public DiscoveredExample FromExample(ExampleBase example)
        {
            var methodInfo = GetFunctionBodyInfo(example);

            string specClassName = methodInfo.DeclaringType.FullName;
            string exampleMethodName = methodInfo.Name;

            var navigationData = debugInfoProvider.GetNavigationData(specClassName, exampleMethodName);

            var discoveredExample = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = binaryPath,
                SourceFilePath = navigationData.FileName,
                SourceLineNumber = navigationData.MinLineNumber,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            return discoveredExample;
        }

        readonly string binaryPath;
        readonly IDebugInfoProvider debugInfoProvider;
        readonly Dictionary<string, BaseExampleBodyGetter> typeNameToBodyGetterMap;

        MethodInfo GetFunctionBodyInfo(ExampleBase example)
        {
            string exampleTypeName = example.GetType().Name;

            BaseExampleBodyGetter getFunctionBodyInfo;

            bool hasGetterForType = typeNameToBodyGetterMap.TryGetValue(exampleTypeName, out getFunctionBodyInfo);

            if (hasGetterForType)
            {
                var info = getFunctionBodyInfo(example);

                return info;
            }
            else
            {
                throw new ArgumentOutOfRangeException("example", String.Format("Unexpected example type: {0}", exampleTypeName));
            }
        }

        delegate MethodInfo BaseExampleBodyGetter(ExampleBase baseExample);

        static MethodInfo GetExampleBodyInfo(ExampleBase baseExample)
        {
            // core logic taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Discoverer.cs

            const string actionPrivateFieldName = "action";

            Example example = (Example)baseExample;

            var action = example.GetType()
                .GetField(actionPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as Action;

            var info = action.Method;

            return info;
        }

        static MethodInfo GetMethodExampleBodyInfo(ExampleBase baseExample)
        {
            // core logic taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Discoverer.cs

            const string methodInfoPrivateFieldName = "method";

            MethodExample example = (MethodExample)baseExample;

            var info = example.GetType()
                .GetField(methodInfoPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as MethodInfo;

            return info;
        }

        static MethodInfo GetAsyncExampleBodyInfo(ExampleBase baseExample)
        {
            const string asyncActionPrivateFieldName = "asyncAction";

            AsyncExample example = (AsyncExample)baseExample;

            var asyncAction = example.GetType()
                .GetField(asyncActionPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as Func<Task>;

            var info = asyncAction.Method;

            return info;
        }

        static MethodInfo GetAsyncMethodExampleBodyInfo(ExampleBase baseExample)
        {
            const string methodInfoPrivateFieldName = "method";

            AsyncMethodExample example = (AsyncMethodExample)baseExample;

            var info = example.GetType()
                .GetField(methodInfoPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as MethodInfo;

            return info;
        }
    }
}
