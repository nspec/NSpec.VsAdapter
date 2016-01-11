using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Discovery;
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
        // Do not move the following spec classes around, to avoid rewriting line numbers

        class ParentSpec : nspec
        {
            [Tag("Tag-1A Tag-1B")]
            void method_context_1()
            { // # 26
                it["parent example 1A"] = () => true.should_be_true();

                it["parent example 1B"] = () => true.should_be_true();
            } // #30

            void method_context_2()
            { // # 33
                it["parent example 2A"] = () => true.should_be_true();
            } // # 35
        }

        [Tag("Child")]
        class ChildSpec : ParentSpec
        {
            [Tag("Child-example-skipped")]
            void method_context_3()
            { // # 43
                it["child example skipped"] = todo;
            } // # 45
        }

        // End of un-movable classes

        DebugInfoProvider provider;

        AutoSubstitute autoSubstitute;
        readonly Dictionary<string, Dictionary<string, DiaNavigationData>> navDataByClassAndMethodName;
        readonly string thisFilePath;

        public DebugInfoProvider_desc()
        {
            thisFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

            navDataByClassAndMethodName = new Dictionary<string,Dictionary<string,DiaNavigationData>>
            {
                { 
                    typeof(ParentSpec).FullName, 
                    new Dictionary<string,DiaNavigationData>()
                    {
                        {
                            "method_context_1",
                            new DiaNavigationData(thisFilePath, 26, 30)
                        },
                        {
                            "method_context_2",
                            new DiaNavigationData(thisFilePath, 33, 35)
                        },
                    }
                },
                { 
                    typeof(ChildSpec).FullName, 
                    new Dictionary<string,DiaNavigationData>()
                    {
                        {
                            "method_context_3",
                            new DiaNavigationData(thisFilePath, 43, 45)
                        },
                    }
                },
            };
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            string binaryPath = Assembly.GetExecutingAssembly().Location;

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
            navDataByClassAndMethodName.Keys.Do(declaringClassName =>
                {
                    var methodInfos = navDataByClassAndMethodName[declaringClassName];

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
