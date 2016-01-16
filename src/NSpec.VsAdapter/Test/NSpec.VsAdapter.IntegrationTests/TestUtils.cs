using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    static class TestUtils
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
    }
}
