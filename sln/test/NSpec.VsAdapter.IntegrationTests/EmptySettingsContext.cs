using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    class EmptyRunSettings : IRunSettings
    {
        public ISettingsProvider GetSettings(string settingsName)
        {
            return new AdapterSettingsProvider();
        }

        public string SettingsXml
        {
            get { throw new DummyTestException(); }
        }
    }

    class EmptyDiscoveryContext : IDiscoveryContext
    {
        public EmptyDiscoveryContext()
        {
            RunSettings = new EmptyRunSettings();
        }

        public IRunSettings RunSettings { get; private set; }
    }

    class EmptyRunContext : EmptyDiscoveryContext, IRunContext
    {
        public ITestCaseFilterExpression GetTestCaseFilter(
            IEnumerable<string> supportedProperties, 
            Func<string, TestProperty> propertyProvider)
        {
            throw new NotImplementedException();
        }

        public bool InIsolation
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsBeingDebugged
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDataCollectionEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool KeepAlive
        {
            get { throw new NotImplementedException(); }
        }

        public string SolutionDirectory
        {
            get { throw new NotImplementedException(); }
        }

        public string TestRunDirectory
        {
            get { throw new NotImplementedException(); }
        }
    }
}
