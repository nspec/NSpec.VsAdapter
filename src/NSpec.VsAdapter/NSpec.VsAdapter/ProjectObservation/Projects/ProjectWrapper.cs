using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public class ProjectWrapper : IProjectWrapper
    {
        public ProjectWrapper(Project autoProject)
        {
            this.autoProject = autoProject;
        }

        public string OutputDirPath
        {
            get { return GetOutputDirPath(autoProject); }
        }

        public string OutputFileName
        {
            get { return GetOutputFileName(autoProject); }
        }

        readonly Project autoProject;

        static string GetOutputDirPath(Project autoProject)
        {
            string projectAbsolutePath = autoProject.Properties.Item("FullPath").Value.ToString();

            // TODO instead of ActiveConfiguration, it should retrieve configuration actually built,
            // from an IVsCfg field that should be carried over from ProjectBuildInfo:
            // IVsCfg.get_DisplayName() returns "<configname>|<platformname>"
            // ConfigurationManager.Platform(platformname) or .ConfigurationRow(configname) return Configuration collections

            string configurationRelativeOutputPath = autoProject.ConfigurationManager.ActiveConfiguration
                .Properties.Item("OutputPath").Value.ToString();

            string absoluteOutputDirPath = Path.Combine(projectAbsolutePath, configurationRelativeOutputPath);

            return absoluteOutputDirPath;
        }

        static string GetOutputFileName(Project autoProject)
        {
            string outputFileName = autoProject.Properties.Item("OutputFileName").Value.ToString();

            return outputFileName;
        }
    }
}
