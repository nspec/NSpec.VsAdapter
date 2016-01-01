using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter
{
    public interface ITestDllNotifier
    {
        IObservable<IEnumerable<string>> PathStream { get; }
    }
}
