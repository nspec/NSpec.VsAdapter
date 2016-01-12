using NSpec;
using SampleSystem;
using System;

namespace SampleSpecs
{
    class ParentSpec : nspec
    {
        protected SystemUnderTest systemUnderTest;

        void before_each()
        {
            systemUnderTest = new SystemUnderTest();
        }

        [Tag("Tag-1A Tag-1B")]
        void method_context_1()
        { // # 18
            it["parent example 1A"] = () => systemUnderTest.IsAlwaysTrue().should_be_true();

            it["parent example 1B"] = () => systemUnderTest.IsAlwaysTrue().should_be_true();
        } // # 22

        void method_context_2()
        { // # 25
            it["parent example 2A"] = () => systemUnderTest.IsAlwaysTrue().should_be_true();
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
            it["child example 4A"] = () => systemUnderTest.IsAlwaysTrue().should_be_true();
        } // # 42
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers
}
