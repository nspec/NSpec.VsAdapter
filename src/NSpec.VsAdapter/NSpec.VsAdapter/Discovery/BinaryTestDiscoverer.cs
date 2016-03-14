using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class BinaryTestDiscoverer : IBinaryTestDiscoverer
    {
        public BinaryTestDiscoverer(IAppDomainFactory appDomainFactory, IProxyableFactory<IProxyableTestDiscoverer> proxyableFactory, 
            IFileService fileService)
        {
            this.appDomainFactory = appDomainFactory;
            this.proxyableFactory = proxyableFactory;
            this.fileService = fileService;
        }

        public IEnumerable<DiscoveredExample> Discover(string binaryPath, 
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            if (!NSpecLibraryFound(binaryPath))
            {
                logger.Info(String.Format("Skipping binary '{0}' because it does not reference nspec library", binaryPath));

                return new DiscoveredExample[0];
            }

            logger.Info(String.Format("Discovering tests in binary '{0}'", binaryPath));

            IEnumerable<DiscoveredExample> discoveredExamples;

            try
            {
                using (var targetDomain = appDomainFactory.Create(binaryPath))
                using (var proxyableTestDiscoverer = proxyableFactory.CreateProxy(targetDomain))
                {
                    discoveredExamples = proxyableTestDiscoverer.Discover(binaryPath, crossDomainLogger);
                }
            }
            catch (Exception ex)
            {
                discoveredExamples = new DiscoveredExample[0];

                // report problem and return for the next assembly, without crashing the test discovery process
                var message = String.Format("Exception thrown while discovering tests in binary '{0}'", binaryPath);
                logger.Error(ex, message);
            }

            logger.Info(String.Format("Found {0} tests in binary '{1}'", discoveredExamples.Count(), binaryPath));

            return discoveredExamples;
        }

        bool NSpecLibraryFound(string binaryPath)
        {
            string binaryDirectoryPath = Path.GetDirectoryName(binaryPath);

            string nspecLibraryPath = Path.Combine(binaryDirectoryPath, "nspec.dll");

            return fileService.Exists(nspecLibraryPath);
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IProxyableFactory<IProxyableTestDiscoverer> proxyableFactory;
        readonly IFileService fileService;
    }
}
