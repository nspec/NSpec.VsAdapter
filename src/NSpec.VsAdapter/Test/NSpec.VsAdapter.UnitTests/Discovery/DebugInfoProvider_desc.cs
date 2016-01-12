using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.UnitTests.Discovery.SampleSpecs;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery
{
    [TestFixture]
    [Category("DebugInfoProvider")]
    public class DebugInfoProvider_desc
    {
        DebugInfoProvider provider;

        AutoSubstitute autoSubstitute;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            string binaryPath = SpecSampleFileInfo.ThisBinaryFilePath;

            var logger = autoSubstitute.Resolve<ISerializableLogger>();

            provider = new DebugInfoProvider(binaryPath, logger);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_return_navigation_data()
        {
            SampleTestCaseDebugInfo.ByClassMethodName.Keys.Do(declaringClassName =>
                {
                    var methodInfos = SampleTestCaseDebugInfo.ByClassMethodName[declaringClassName];

                    methodInfos.Keys.Do(methodName =>
                        {
                            DiaNavigationData expected = methodInfos[methodName];

                            DiaNavigationData actual = provider.GetNavigationData(declaringClassName, methodName);

                            actual.ShouldBeEquivalentTo(expected, 
                                "ClassName: {0}, MethodName: {1}", declaringClassName, methodName);
                        });
                });
        }
    }
}
