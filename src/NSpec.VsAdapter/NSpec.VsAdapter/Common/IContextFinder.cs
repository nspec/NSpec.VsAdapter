using NSpec.Domain;

namespace NSpec.VsAdapter.Common
{
    public interface IContextFinder
    {
        ContextCollection BuildContextCollection(string binaryPath);
    }
}
