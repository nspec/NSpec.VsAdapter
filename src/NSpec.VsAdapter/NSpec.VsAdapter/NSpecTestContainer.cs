using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using Microsoft.VisualStudio.TestWindow.Extensibility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class NSpecTestContainer : ITestContainer
    {
        public NSpecTestContainer(
            ITestContainerDiscoverer containerDiscoverer, 
            string sourcePath, 
            IEnumerable<Guid> debugEngines,
            IFileService fileService)
        {
            this.containerDiscoverer = containerDiscoverer;
            this.sourcePath = sourcePath;
            this.debugEngines = debugEngines;
            this.fileService = fileService;

            timeStamp = emptyStamp;
        }

        private NSpecTestContainer(NSpecTestContainer other)
            : this(other.containerDiscoverer, other.sourcePath, other.debugEngines, other.fileService)
        {
            timeStamp = other.timeStamp;
        }

        public ITestContainerDiscoverer Discoverer
        {
            get { return containerDiscoverer; }
        }

        public string Source
        {
            get { return sourcePath; }
        }

        public IEnumerable<Guid> DebugEngines
        {
            get { return debugEngines; }
        }

        public FrameworkVersion TargetFramework
        {
            get { return FrameworkVersion.None; }
        }

        public Architecture TargetPlatform
        {
            get { return Architecture.AnyCPU; }
        }

        public bool IsAppContainerTestContainer
        {
            get { return false; }
        }

        public IDeploymentData DeployAppContainer()
        {
            return null;
        }

        public int CompareTo(ITestContainer other)
        {
            const int nonMatching = -1;
            const int matching = 0;

            NSpecTestContainer otherContainer = other as NSpecTestContainer;

            if (otherContainer == null)
            {
                return nonMatching;
            }

            int sourceCompare = String.Compare(sourcePath, otherContainer.Source, StringComparison.OrdinalIgnoreCase);

            if (sourceCompare != 0)
            {
                return nonMatching;
            }

            EnsureTimeStamp();
            otherContainer.EnsureTimeStamp();

            return (timeStamp == otherContainer.timeStamp ? matching : nonMatching);
        }

        private void EnsureTimeStamp()
        {
            if (timeStamp == emptyStamp && fileService.Exists(sourcePath))
            {
                timeStamp = fileService.LastModified(sourcePath);
            }
        }

        public ITestContainer Snapshot()
        {
            return new NSpecTestContainer(this);
        }

        readonly ITestContainerDiscoverer containerDiscoverer;
        readonly string sourcePath;
        readonly IEnumerable<Guid> debugEngines;
        readonly IFileService fileService;
        DateTime timeStamp;

        static readonly DateTime emptyStamp = DateTime.MinValue;
    }
}
