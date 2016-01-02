using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.Shell.Interop;
using NSpec.VsAdapter.ProjectObservation;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.ProjectObservation
{
    [TestFixture]
    [Category("ProjectConverter")]
    public class ProjectConverter_desc
    {
        ProjectConverter converter;

        AutoSubstitute autoSubstitute;
        IProjectWrapperFactory projectWrapperFactory;
        IFileService fileService;

        ProjectInfo someProjectInfo;
        IVsHierarchy someHierarchy;
        IProjectWrapper someDummyProject;

        const string someOutputPath = @".\some\test\output\path";
        const string someDllName = "test-library.dll";
        readonly string nspecOutputPath;
        readonly string someDllPath;

        public ProjectConverter_desc()
        {
            nspecOutputPath = Path.Combine(someOutputPath, "NSpec.dll");
            someDllPath = Path.Combine(someOutputPath, someDllName);
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            projectWrapperFactory = autoSubstitute.Resolve<IProjectWrapperFactory>();

            fileService = autoSubstitute.Resolve<IFileService>();

            IProjectWrapper notAProject = null;
            projectWrapperFactory.Create(Arg.Any<IVsHierarchy>()).Returns(notAProject);

            fileService.Exists(Arg.Any<string>()).Returns(false);

            converter = autoSubstitute.Resolve<ProjectConverter>();

            someProjectInfo = new ProjectInfo();
            someHierarchy = autoSubstitute.Resolve<IVsHierarchy>();
            someDummyProject = autoSubstitute.Resolve<IProjectWrapper>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_not_convert_when_node_is_not_a_project()
        {
            converter.ToTestDllPath(someProjectInfo).Should().BeNull();
        }

        [Test]
        public void it_should_not_convert_when_nspec_dll_is_not_in_output_dir()
        {
            someProjectInfo.Hierarchy = someHierarchy;

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            converter.ToTestDllPath(someProjectInfo).Should().BeNull();
        }

        [Test]
        public void it_should_not_convert_when_output_dll_is_not_found()
        {
            someProjectInfo.Hierarchy = someHierarchy;

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            someDummyProject.OutputDirPath.Returns(someOutputPath);
            someDummyProject.OutputFileName.Returns(someDllName);

            fileService.Exists(nspecOutputPath).Returns(true);

            converter.ToTestDllPath(someProjectInfo).Should().BeNull();
        }

        [Test]
        public void it_should_return_dll_path_when_nspec_dll_is_in_output_dir()
        {
            someProjectInfo.Hierarchy = someHierarchy;

            projectWrapperFactory.Create(someHierarchy).Returns(someDummyProject);

            someDummyProject.OutputDirPath.Returns(someOutputPath);
            someDummyProject.OutputFileName.Returns(someDllName);

            fileService.Exists(nspecOutputPath).Returns(true);
            fileService.Exists(someDllPath).Returns(true);

            converter.ToTestDllPath(someProjectInfo).Should().Be(someDllPath);
        }
    }
}
