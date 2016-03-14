using Microsoft.VisualStudio.Shell.Interop;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public class ProjectConverter : IProjectConverter
    {
        public ProjectConverter(IProjectWrapperFactory projectWrapperFactory, IFileService fileService)
        {
            this.projectWrapperFactory = projectWrapperFactory;
            this.fileService = fileService;
        }

        public string ToTestDllPath(ProjectInfo projectInfo)
        {
            const string noTestDllPath = null;

            var projectWrapper = projectWrapperFactory.Create(projectInfo.Hierarchy);

            if (projectWrapper == null)
            {
                return noTestDllPath;
            }

            string outputDirPath = projectWrapper.OutputDirPath;
            
            const string dllFilename = "NSpec.dll";

            string nspecDllPath = Path.Combine(outputDirPath, dllFilename);

            bool hasNSpecDllReference = fileService.Exists(nspecDllPath);

            if (! hasNSpecDllReference)
            {
                return noTestDllPath;
            }

            string outputFileName = projectWrapper.OutputFileName;

            string testDllPath = Path.Combine(outputDirPath, outputFileName);

            return fileService.Exists(testDllPath) ? testDllPath : noTestDllPath; 
        }

        readonly IProjectWrapperFactory projectWrapperFactory;
        readonly IFileService fileService;
    }
}
