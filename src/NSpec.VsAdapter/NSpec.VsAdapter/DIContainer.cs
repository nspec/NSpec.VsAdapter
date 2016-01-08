using Autofac;
using NSpec.VsAdapter.NSpecModding;
using NSpec.VsAdapter.ProjectObservation;
using NSpec.VsAdapter.ProjectObservation.Projects;
using NSpec.VsAdapter.ProjectObservation.Solution;
using NSpec.VsAdapter.TestAdapter;
using NSpec.VsAdapter.TestExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    class DIContainer : IDisposable
    {
        private DIContainer()
        {
            var builder = new ContainerBuilder();

            RegisterContainerDiscoverer(builder);
            RegisterDiscoverer(builder);
            RegisterExecutor(builder);

            container = builder.Build();

            containerDiscovererScopeHolder = new Lazy<ILifetimeScope>(() => container.BeginLifetimeScope());
            discovererScopeHolder = new Lazy<ILifetimeScope>(() => container.BeginLifetimeScope());
            executorScopeHolder = new Lazy<ILifetimeScope>(() => container.BeginLifetimeScope());
        }

        public ILifetimeScope ContainerDiscoverer
        {
            get { return containerDiscovererScopeHolder.Value; }
        }

        readonly Lazy<ILifetimeScope> containerDiscovererScopeHolder;

        public ILifetimeScope Discoverer
        {
            get { return discovererScopeHolder.Value; }
        }

        readonly Lazy<ILifetimeScope> discovererScopeHolder;

        public ILifetimeScope Executor
        {
            get { return executorScopeHolder.Value; }
        }

        readonly Lazy<ILifetimeScope> executorScopeHolder;

        public void Dispose()
        {
            container.Dispose();
        }

        readonly IContainer container;

        static void RegisterContainerDiscoverer(ContainerBuilder builder)
        {
            builder.RegisterType<NSpecTestDllNotifier>().As<ITestDllNotifier>().InstancePerLifetimeScope();
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
            builder.RegisterType<AppDomainFactory>().As<IAppDomainFactory>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MarshalingFactory<>)).As(typeof(IMarshalingFactory<>)).InstancePerLifetimeScope();
            builder.RegisterType<CrossDomainCollector>().As<ICrossDomainCollector>().InstancePerLifetimeScope();
            builder.RegisterType<CrossDomainTestDiscoverer>().As<ICrossDomainTestDiscoverer>().InstancePerLifetimeScope();
            builder.RegisterType<TestCaseMapper>().As<ITestCaseMapper>().InstancePerLifetimeScope();
            builder.RegisterType<AdapterInfo>().As<IAdapterInfo>().InstancePerLifetimeScope();
        }

        static void RegisterExecutor(ContainerBuilder builder)
        {
            // TODO RegisterExecutor in DI
        }

        public static DIContainer Instance 
        {
            get { return instanceHolder.Value; }
        }

        static readonly Lazy<DIContainer> instanceHolder = new Lazy<DIContainer>(() => new DIContainer(), false);
    }
}
