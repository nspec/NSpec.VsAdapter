﻿using NSpec;
using Shouldly;
using System;

namespace SampleSpecs
{
    class ParentSpec : nspec
    {
        protected SampleSystem.SystemUnderTest systemUnderTest;

        void before_each()
        {
            systemUnderTest = new SampleSystem.SystemUnderTest();
        }

        [Tag("Tag-1A Tag-1B")]
        void method_context_1()
        { // # 18
            it["parent example 1A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 19

            it["parent example 1B"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 21
        } // # 22

        void method_context_2()
        { // # 25
            it["parent example 2A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 26
        } // # 27
    }

    [Tag("Tag-Child")]
    class ChildSpec : ParentSpec
    {
        [Tag("Tag-Child-example-skipped")]
        void method_context_3()
        { // # 35
            it["child example 3A skipped"] = todo; // # 36
        } // # 37

        [Tag("Tag_with_underscores")]
        void method_context_4()
        { // # 41
            it["child example 4A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 42
        } // # 43

        void method_context_5()
        { // # 46
            context["sub context 5-1"] = () =>
            {
                it["child example 5-1A failing"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeFalse(); // # 49

                it["child example 5-1B"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 51
            };
        } // # 53
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers

    public class DummyPublicClass
    {
    }
}
