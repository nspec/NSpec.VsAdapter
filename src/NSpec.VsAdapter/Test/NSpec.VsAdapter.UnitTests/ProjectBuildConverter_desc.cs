using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.Shell.Interop;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests
{
    [TestFixture]
    [Category("ProjectBuildConverter")]
    public class ProjectBuildConverter_desc
    {
        ProjectBuildConverter converter;

        AutoSubstitute autoSubstitute;
        IProjectWrapperFactory projectWrapperFactory;
        IFileService fileService;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            projectWrapperFactory = autoSubstitute.Resolve<IProjectWrapperFactory>();

            fileService = autoSubstitute.Resolve<IFileService>();

            IProjectWrapper notAProject = null;
            projectWrapperFactory.Create(Arg.Any<IVsHierarchy>()).Returns(notAProject);

            fileService.Exists(Arg.Any<string>()).Returns(false);

            converter = autoSubstitute.Resolve<ProjectBuildConverter>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_not_convert_when_node_is_not_a_project()
        {
            var someBuildInfo = new ProjectBuildInfo();

            converter.ToTestDllPath(someBuildInfo).Should().BeNull();
        }

        [Test]
        public void it_should_not_convert_when_nspec_dll_is_not_in_output_dir()
        {
            var someBuildInfo = new ProjectBuildInfo();
            var someHierarchy = autoSubstitute.Resolve<IVsHierarchy>();
            someBuildInfo.Hierarchy = someHierarchy;

            IProjectWrapper someDummyProject = autoSubstitute.Resolve<IProjectWrapper>();

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            converter.ToTestDllPath(someBuildInfo).Should().BeNull();
        }

        [Test]
        public void it_should_return_dll_path_when_nspec_dll_is_in_output_dir()
        {
            var someBuildInfo = new ProjectBuildInfo();
            var someHierarchy = autoSubstitute.Resolve<IVsHierarchy>();
            someBuildInfo.Hierarchy = someHierarchy;

            IProjectWrapper someDummyProject = autoSubstitute.Resolve<IProjectWrapper>();
            string someOutputPath = @".\some\test\output\path";
            string nspecOutputPath = Path.Combine(someOutputPath, "NSpec.dll");
            string someDllName = "test-library.dll";
            string someDllPath = Path.Combine(someOutputPath, someDllName);

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            someDummyProject.OutputDirPath.Returns(someOutputPath);
            someDummyProject.OutputFileName.Returns(someDllName);

            fileService.Exists(nspecOutputPath).Returns(true);

            converter.ToTestDllPath(someBuildInfo).Should().Be(someDllPath);
        }
    }
}
