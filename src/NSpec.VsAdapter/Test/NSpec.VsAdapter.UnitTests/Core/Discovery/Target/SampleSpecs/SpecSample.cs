using NSpec;
using System;
using System.Reflection;

namespace NSpec.VsAdapter.UnitTests.Core.Discovery.Target.SampleSpecs
{
    class ParentSpec : nspec
    {
        protected bool valueUnderTest;

        void before_each()
        {
            valueUnderTest = true;
        }

        [Tag("Tag-1A Tag-1B")]
        void method_context_1()
        { // # 18
            it["parent example 1A"] = () => valueUnderTest.should_be_true();

            it["parent example 1B"] = () => valueUnderTest.should_be_true();
        } // # 22

        void method_context_2()
        { // # 25
            it["parent example 2A"] = () => valueUnderTest.should_be_true();
        } // # 27
    }

    [Tag("Tag-Child")]
    class ChildSpec : ParentSpec
    {
        [Tag("Tag-Child-example-skipped")]
        void method_context_3()
        { // # 35
            it["child example 3A skipped"] = todo;
        } // # 37

        void method_context_4()
        { // # 40
            it["child example 4A"] = () => valueUnderTest.should_be_true();
        } // # 42
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers

    static class SpecSampleFileInfo
    {
        public static readonly string ThisSourceCodeFilePath;
        public static readonly string ThisBinaryFilePath;

        static SpecSampleFileInfo()
        {
            ThisSourceCodeFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

            ThisBinaryFilePath = Assembly.GetExecutingAssembly().Location;
        }

    }
}
