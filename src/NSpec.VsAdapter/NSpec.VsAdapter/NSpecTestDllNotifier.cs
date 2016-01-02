using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class NSpecTestDllNotifier : ITestDllNotifier, IDisposable
    {
        public NSpecTestDllNotifier(
            IProjectBuildNotifier projectBuildNotifier, 
            IProjectBuildConverter projectBuildConverter)
        {
            const string notATestDllPath = null;

            var hotPathStream = projectBuildNotifier.BuildStream
                .Select(buildInfos =>
                {
                    IEnumerable<string> dllPaths = buildInfos
                        .Select(projectBuildConverter.ToTestDllPath)
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
