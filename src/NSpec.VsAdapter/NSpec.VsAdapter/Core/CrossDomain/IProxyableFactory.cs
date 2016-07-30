namespace NSpec.VsAdapter.Core.CrossDomain
{
    public interface IProxyableFactory<TProxyable>
    {
        TProxyable CreateProxy(ITargetAppDomain targetDomain);
    }
}
