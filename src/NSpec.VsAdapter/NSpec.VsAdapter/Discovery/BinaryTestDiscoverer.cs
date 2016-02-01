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
        public BinaryTestDiscoverer(ICrossDomainCollector crossDomainCollector, IFileService fileService)
        {
            this.crossDomainCollector = crossDomainCollector;
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

            try
            {
                logger.Info(String.Format("Discovering tests in binary '{0}'", binaryPath));

                var collectorInvocation = new CollectorInvocation(binaryPath, crossDomainLogger);

                var discoveredExamples = crossDomainCollector.Run(binaryPath, collectorInvocation.Collect);

                logger.Info(String.Format("Found {0} tests", discoveredExamples.Count()));

                return discoveredExamples;
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test discovery process

                var message = String.Format("Exception thrown while discovering tests in binary '{0}'", binaryPath);
                
                logger.Error(ex, message);

                return new DiscoveredExample[0];
            }
        }

        bool NSpecLibraryFound(string binaryPath)
        {
            string binaryDirectoryPath = Path.GetDirectoryName(binaryPath);

            string nspecLibraryPath = Path.Combine(binaryDirectoryPath, "nspec.dll");

            return fileService.Exists(nspecLibraryPath);
        }

        readonly ICrossDomainCollector crossDomainCollector;
        readonly IFileService fileService;
    }
}
