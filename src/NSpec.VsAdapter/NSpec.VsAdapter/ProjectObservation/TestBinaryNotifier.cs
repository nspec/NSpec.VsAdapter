using NSpec.VsAdapter.ProjectObservation;
using NSpec.VsAdapter.ProjectObservation.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class TestBinaryNotifier : ITestBinaryNotifier, IDisposable
    {
        public TestBinaryNotifier(
            IProjectNotifier projectNotifier, 
            IProjectBuildNotifier buildNotifier,
            IProjectConverter projectConverter)
        {
            var mapProjectInfosToPaths = MapProjectInfosToPaths(projectConverter);

            var testProjectPathStream = projectNotifier.ProjectStream
                .Select(mapProjectInfosToPaths)
                .Publish() // this will be subscribed multiple times: avoid re-subscription side-effect
                .RefCount();

            var testBuildPathStream = buildNotifier.BuildStream
                .Select(projectConverter.ToTestDllPath)
                .Where(ConversionSuccess);

            var buildRefreshPathStream = testProjectPathStream
                .Sample(testBuildPathStream);

            var hotPathStream = testProjectPathStream
                .Merge(buildRefreshPathStream)
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

        static Func<IEnumerable<ProjectInfo>, IEnumerable<string>> MapProjectInfosToPaths(IProjectConverter projectConverter)
        {
            return projectInfos =>
            {
                IEnumerable<string> dllPaths =
                    from info in projectInfos
                    let path = projectConverter.ToTestDllPath(info)
                    where ConversionSuccess(path)
                    select path;

                return dllPaths;
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool ConversionSuccess(string path)
        {
            const string notATestDllPath = null;

            return (path != notATestDllPath);
        }

    }
}
