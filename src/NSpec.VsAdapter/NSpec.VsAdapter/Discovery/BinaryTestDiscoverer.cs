using NSpec.VsAdapter.Common;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NSpec.VsAdapter.Discovery
{
    public class BinaryTestDiscoverer : IBinaryTestDiscoverer
    {
        public BinaryTestDiscoverer(ICrossDomainRunner<IProxyableTestDiscoverer, DiscoveredExample[]> remoteRunner,
            IFileService fileService)
        {
            this.remoteRunner = remoteRunner;
            this.fileService = fileService;
        }

        public IEnumerable<DiscoveredExample> Discover(string binaryPath,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            if (!NSpecLibraryFound(binaryPath))
            {
                logger.Debug(String.Format("Skipping binary '{0}' because it does not reference nspec library", binaryPath));

                return new DiscoveredExample[0];
            }
            else
            {
                var discoveryOperation = new BinaryDiscoveryOperation(binaryPath, crossDomainLogger);

                return RunOperationRemotely(discoveryOperation, binaryPath, logger);
            }
        }

        IEnumerable<DiscoveredExample> RunOperationRemotely(
            IDiscoveryOperation discoveryOperation, string binaryPath, IOutputLogger logger)
        {
            logger.Info(String.Format("Discovering tests in binary '{0}'", binaryPath));

            IEnumerable<DiscoveredExample> discoveredExamples = remoteRunner.Run(binaryPath, discoveryOperation.Run,
                (ex, path) =>
                {
                    // report problem and return for the next assembly, without crashing the test discovery process
                    var message = String.Format("Exception thrown while discovering tests in binary '{0}'", binaryPath);
                    logger.Error(ex, message);

                    return new DiscoveredExample[0];
                });

            logger.Info(String.Format("Found {0} tests in binary '{1}'", discoveredExamples.Count(), binaryPath));

            return discoveredExamples;
        }

        bool NSpecLibraryFound(string binaryPath)
        {
            string binaryDirectoryPath = Path.GetDirectoryName(binaryPath);

            string nspecLibraryPath = Path.Combine(binaryDirectoryPath, "nspec.dll");

            return fileService.Exists(nspecLibraryPath);
        }

        readonly ICrossDomainRunner<IProxyableTestDiscoverer, DiscoveredExample[]> remoteRunner;
        readonly IFileService fileService;

        interface IDiscoveryOperation
        {
            DiscoveredExample[] Run(IProxyableTestDiscoverer proxyableDiscoverer);
        }

        class BinaryDiscoveryOperation : IDiscoveryOperation
        {
            public BinaryDiscoveryOperation(string binaryPath, ICrossDomainLogger crossDomainLogger)
            {
                this.binaryPath = binaryPath;
                this.crossDomainLogger = crossDomainLogger;
            }

            public DiscoveredExample[] Run(IProxyableTestDiscoverer proxyableDiscoverer)
            {
                return proxyableDiscoverer.Discover(binaryPath, crossDomainLogger);
            }

            readonly string binaryPath;
            readonly ICrossDomainLogger crossDomainLogger;
        }
    }
}
