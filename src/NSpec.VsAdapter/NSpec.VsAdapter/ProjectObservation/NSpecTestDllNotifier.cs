using NSpec.VsAdapter.ProjectObservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class NSpecTestDllNotifier : ITestDllNotifier, IDisposable
    {
        public NSpecTestDllNotifier(
            IProjectNotifier projectNotifier, 
            IProjectConverter projectConverter)
        {
            const string notATestDllPath = null;

            var hotPathStream = projectNotifier.ProjectStream
                .Select(projectInfos =>
                {
                    IEnumerable<string> dllPaths = projectInfos
                        .Select(projectConverter.ToTestDllPath)
                        .Where(path => path != notATestDllPath);

                    return dllPaths;
                })
                .Replay(1);

            hotPathStream.Connect().DisposeWith(disposables);

            PathStream = hotPathStream;
        }

        public IObservable<IEnumerable<string>> PathStream { get; private set; }

        public void Dispose()
        {
            disposables.Dispose();
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();
    }
}
