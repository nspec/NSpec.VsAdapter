using Autofac;
using NSpec.VsAdapter.Common;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using NSpec.VsAdapter.ProjectObservation;
using NSpec.VsAdapter.ProjectObservation.Projects;
using NSpec.VsAdapter.ProjectObservation.Solution;
using NSpec.VsAdapter.Settings;
using NSpec.VsAdapter.TestAdapter.Discovery;
using NSpec.VsAdapter.TestAdapter.Execution;
using NSpec.VsAdapter.TestExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    class DependencyContainer : IDisposable
    {
        private DependencyContainer()
        {
            var builder = new ContainerBuilder();

            RegisterContainerDiscoverer(builder);
            RegisterDiscoverer(builder);
            RegisterExecutor(builder);
            RegisterCommon(builder);

            container = builder.Build();
        }

        public ILifetimeScope BeginScope()
        {
            return container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            container.Dispose();
        }

        readonly IContainer container;

        static void RegisterContainerDiscoverer(ContainerBuilder builder)
        {
            builder.RegisterType<TestBinaryNotifier>().As<ITestBinaryNotifier>().InstancePerLifetimeScope();
            builder.RegisterType<NSpecTestContainerFactory>().As<ITestContainerFactory>().InstancePerLifetimeScope();
            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectNotifier>().As<IProjectNotifier>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectBuildNotifier>().As<IProjectBuildNotifier>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectConverter>().As<IProjectConverter>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectWrapperFactory>().As<IProjectWrapperFactory>().InstancePerLifetimeScope();

            builder.RegisterType<SolutionProvider>().As<ISolutionProvider>().InstancePerLifetimeScope();
            builder.RegisterType<SolutionNotifier>().As<ISolutionNotifier>().InstancePerLifetimeScope();
            builder.RegisterType<SolutionBuildManagerProvider>().As<ISolutionBuildManagerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectEnumerator>().As<IProjectEnumerator>().InstancePerLifetimeScope();
        }

        static void RegisterDiscoverer(ContainerBuilder builder)
        {
            builder.RegisterType<MultiSourceTestDiscovererFactory>().As<IMultiSourceTestDiscovererFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProxyableTestDiscovererFactory>().As<IProxyableFactory<IProxyableTestDiscoverer>>().InstancePerLifetimeScope();
            builder.RegisterType<BinaryTestDiscoverer>().As<IBinaryTestDiscoverer>().InstancePerLifetimeScope();
            builder.RegisterType<TestCaseMapper>().As<ITestCaseMapper>().InstancePerLifetimeScope();
        }

        static void RegisterExecutor(ContainerBuilder builder)
        {
            builder.RegisterType<MultiSourceTestExecutorFactory>().As<IMultiSourceTestExecutorFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProxyableTestExecutorFactory>().As<IProxyableFactory<IProxyableTestExecutor>>().InstancePerLifetimeScope();
            builder.RegisterType<BinaryTestExecutor>().As<IBinaryTestExecutor>().InstancePerLifetimeScope();
            builder.RegisterType<ProgressRecorderFactory>().As<IProgressRecorderFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TestResultMapper>().As<ITestResultMapper>().InstancePerLifetimeScope();
        }

        static void RegisterCommon(ContainerBuilder builder)
        {
            builder.RegisterType<AppDomainFactory>().As<IAppDomainFactory>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(CrossDomainRunner<,>)).As(typeof(ICrossDomainRunner<,>)).InstancePerLifetimeScope();
            builder.RegisterType<AdapterInfo>().As<IAdapterInfo>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().InstancePerLifetimeScope();
        }

        public static DependencyContainer Instance 
        {
            get { return instanceHolder.Value; }
        }

        static readonly Lazy<DependencyContainer> instanceHolder = new Lazy<DependencyContainer>(() => new DependencyContainer(), false);
    }
}
