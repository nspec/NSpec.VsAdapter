using System.Reflection;

namespace NSpec.VsAdapter
{
    public class AdapterInfo : IAdapterInfo
    {
        public string Name
        {
            get { return "NSpec VS Adapter"; }
        }

        public string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
    }
}
