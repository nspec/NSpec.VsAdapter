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
        // used by Visual Studio test infrastructure
        public NSpecTestContainerDiscoverer() : this(
            DIContainer.Instance.ContainerDiscoverer.Resolve<ITestDllNotifier>(),
            DIContainer.Instance.ContainerDiscoverer.Resolve<ITestContainerFactory>())
        { 
        }

        // used to test this adapter
        public NSpecTestContainerDiscoverer(
            ITestDllNotifier testDllNotifier, 
            ITestContainerFactory containerFactory)
        {
            DIContainer.Instance.ContainerDiscoverer.DisposeWith(disposables);

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
            return dllPaths.Select(path => containerFactory.Create(this, path));
        }

        void RaiseTestContainersUpdated()
        {
            var eventHandler = TestContainersUpdated;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        readonly IObservable<IEnumerable<ITestContainer>> containerStream;
        readonly ITestContainerFactory containerFactory;

        readonly CompositeDisposable disposables = new CompositeDisposable();
    }
}
