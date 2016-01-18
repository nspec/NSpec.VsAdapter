using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Common
{
    public interface IContextFinder
    {
        ContextCollection BuildContextCollection(string binaryPath);
    }
}
