using NSpec;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace SampleAsyncSpecs
{
    public class AsyncSpec : nspec
    {
        protected SampleAsyncSystem.AsyncSystemUnderTest systemUnderTest;

        void before_each()
        {
            systemUnderTest = new SampleAsyncSystem.AsyncSystemUnderTest();
        }

        async Task it_async_method_example()
        { // # 18
            bool actual = await systemUnderTest.IsAlwaysTrueAsync();

            actual.ShouldBeTrue();
        } // # 22

        void method_context()
        { // # 25
            itAsync["async context example"] = async () => // # 26
            {
                bool actual = await systemUnderTest.IsAlwaysTrueAsync();

                actual.ShouldBeTrue();
            };
        } // # 32
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers

    public class DummyPublicClass
    {
    }
}
