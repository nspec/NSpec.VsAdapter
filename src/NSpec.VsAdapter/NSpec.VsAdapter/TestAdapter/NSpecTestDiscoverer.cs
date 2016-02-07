using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    [FileExtension(Constants.DllExtension)]
    [FileExtension(Constants.ExeExtension)]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class NSpecTestDiscoverer : ITestDiscoverer, IDisposable
    {
        // Visual Studio test infrastructure requires a default constructor
        public NSpecTestDiscoverer()
        {
            var scope = DIContainer.Instance.BeginScope();

            disposable = scope;

            multiSourceTestDiscovererFactory = scope.Resolve<IMultiSourceTestDiscovererFactory>();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            var multiSourceTestDiscoverer = multiSourceTestDiscovererFactory.Create(sources);

            multiSourceTestDiscoverer.DiscoverTests(discoverySink, logger, discoveryContext);
        }

        readonly IMultiSourceTestDiscovererFactory multiSourceTestDiscovererFactory;
        readonly IDisposable disposable;
    }
}
