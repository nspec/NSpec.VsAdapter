using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class ProjectBuildConverter : IProjectBuildConverter
    {
        public ProjectBuildConverter(IProjectWrapperFactory projectWrapperFactory, IFileService fileService)
        {
            this.projectWrapperFactory = projectWrapperFactory;
            this.fileService = fileService;
        }

        public string ToTestDllPath(ProjectBuildInfo projectBuildInfo)
        {
            const string noTestDllPath = null;

            var projectWrapper = projectWrapperFactory.Create(projectBuildInfo.Hierarchy);

            if (projectWrapper == null)
            {
                return noTestDllPath;
            }

            string outputDirPath = projectWrapper.OutputDirPath;
            
            const string dllFilename = "NSpec.dll";

            string nspecDllPath = Path.Combine(outputDirPath, dllFilename);

            bool hasNSpecDllReference = fileService.Exists(nspecDllPath);

            if (hasNSpecDllReference)
            {
                string outputFileName = projectWrapper.OutputFileName;

                return Path.Combine(outputDirPath, outputFileName);
            }
            else
            {
                return noTestDllPath;
            }
        }

        readonly IProjectWrapperFactory projectWrapperFactory;
        readonly IFileService fileService;
    }
}
