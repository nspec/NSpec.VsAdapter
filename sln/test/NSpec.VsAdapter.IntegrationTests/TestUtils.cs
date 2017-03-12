using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    public static class TestUtils
    {
        public static string FirstCharToUpper(string original)
        {
            if (original == null)
                return null;

            if (original.Length > 1)
                return char.ToUpper(original[0]) + original.Substring(1);

            return original.ToUpper();
        }

        public static string FirstCharToLower(string original)
        {
            if (original == null)
                return null;

            if (original.Length > 1)
                return char.ToLower(original[0]) + original.Substring(1);

            return original.ToLower();
        }

        // adapted from https://github.com/mattflo/NSpec/blob/master/NSpec/Extensions.cs

        public static IEnumerable<TestCase> Do(this IEnumerable<TestCase> source, Action<TestCase> action)
        {
            foreach (var t in source)
                action(t);

            return source;
        }
    }
}
