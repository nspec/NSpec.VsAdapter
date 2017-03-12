namespace NSpec.VsAdapter.Core.CrossDomain
{
    public interface IAppDomainFactory
    {
        ITargetAppDomain Create(string binaryPath);
    }
}
