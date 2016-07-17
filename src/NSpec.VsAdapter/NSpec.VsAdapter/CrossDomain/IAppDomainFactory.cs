namespace NSpec.VsAdapter.CrossDomain
{
    public interface IAppDomainFactory
    {
        ITargetAppDomain Create(string binaryPath);
    }
}
