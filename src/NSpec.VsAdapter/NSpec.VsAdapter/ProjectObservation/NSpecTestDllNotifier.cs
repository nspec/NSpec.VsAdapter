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
            var mapProjectInfosToPaths = MapProjectInfosToPaths(projectConverter);

            var hotPathStream = projectNotifier.ProjectStream
                .Select(mapProjectInfosToPaths)
                .Replay(1);

            hotPathStream.Connect().DisposeWith(disposables);

            PathStream = hotPathStream;
        }

        public IObservable<IEnumerable<string>> PathStream { get; private set; }

        public void Dispose()
        {
            disposables.Dispose();
        }

        static Func<IEnumerable<ProjectInfo>, IEnumerable<string>> MapProjectInfosToPaths(IProjectConverter projectConverter)
        {
            return projectInfos =>
            {
                const string notATestDllPath = null;

                IEnumerable<string> dllPaths =
                    from info in projectInfos
                    let path = projectConverter.ToTestDllPath(info)
                    where path != notATestDllPath
                    select path;

                return dllPaths;
            };
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();
    }
}
