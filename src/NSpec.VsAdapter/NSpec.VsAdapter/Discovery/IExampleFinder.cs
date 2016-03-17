using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    public interface IExampleFinder
    {
        IEnumerable<ExampleBase> Find(string binaryPath);
    }
}
