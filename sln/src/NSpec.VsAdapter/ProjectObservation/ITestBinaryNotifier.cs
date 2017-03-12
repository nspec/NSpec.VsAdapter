using System;
using System.Collections.Generic;

namespace NSpec.VsAdapter.ProjectObservation
{
    public interface ITestBinaryNotifier
    {
        IObservable<IEnumerable<string>> PathStream { get; }
    }
}
