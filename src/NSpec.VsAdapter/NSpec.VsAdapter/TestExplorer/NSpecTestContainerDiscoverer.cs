using Autofac;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using NSpec.VsAdapter.ProjectObservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestExplorer
{
    public class NSpecTestContainerDiscoverer : ITestContainerDiscoverer, IDisposable
    {
        // used by Visual Studio test infrastructure, by integration tests
        public NSpecTestContainerDiscoverer()
        {
            var scope = DIContainer.Instance.BeginScope();

            disposables.Add(scope); ;

            var containerFactory = scope.Resolve<ITestContainerFactory>();
            var testDllNotifier = scope.Resolve<ITestDllNotifier>();

            Initialize(testDllNotifier, containerFactory);
        }

        // used by unit tests
        public NSpecTestContainerDiscoverer(
            ITestDllNotifier testDllNotifier, 
            ITestContainerFactory containerFactory)
        {
            Initialize(testDllNotifier, containerFactory);
        }

        void Initialize(
            ITestDllNotifier testDllNotifier,
            ITestContainerFactory containerFactory)
        {
            this.containerFactory = containerFactory;

            testDllNotifier.PathStream.Subscribe(_ =>
                {
                    RaiseTestContainersUpdated();
                })
                .DisposeWith(disposables);

            var noDllPaths = new string[0];

            var hotContainerStream = testDllNotifier.PathStream
                .StartWith(noDllPaths)
                .Select(MapToContainers)
                .Replay(1);  // "remember" last observation when TestContainers is requested for the first time

            hotContainerStream.Connect().DisposeWith(disposables);

            containerStream = hotContainerStream;
        }

        public Uri ExecutorUri
        {
            get { return Constants.ExecutorUri; }
        }

        public IEnumerable<ITestContainer> TestContainers
        {
            get { return containerStream.Latest().First(); }
        }

        public event EventHandler TestContainersUpdated;

        public void Dispose()
        {
            disposables.Dispose();
        }

        IEnumerable<ITestContainer> MapToContainers(IEnumerable<string> dllPaths)
        {
            var containers = 
                from path in dllPaths
                select containerFactory.Create(this, path);

            return containers;
        }

        void RaiseTestContainersUpdated()
        {
            var eventHandler = TestContainersUpdated;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        ITestContainerFactory containerFactory;
        IObservable<IEnumerable<ITestContainer>> containerStream;

        readonly CompositeDisposable disposables = new CompositeDisposable();
    }
}
