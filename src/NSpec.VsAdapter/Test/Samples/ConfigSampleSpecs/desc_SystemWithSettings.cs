using ConfigSampleSystem;
using NSpec;
using Shouldly;
using System;

namespace ConfigSampleSpecs
{
    class desc_SystemWithSettings : nspec
    {
        SystemWithSettings systemWithSettings;

        void before_each()
        {
            systemWithSettings = new SystemWithSettings();
        }

        void method_context()
        {
            it["should return app settings value"] = () => // #19
            {
                int expected = 123;

                systemWithSettings.AppSettingsProperty.ShouldBe(expected);
            };
        }
    }

    public class DummyPublicClass
    {
    }
}
