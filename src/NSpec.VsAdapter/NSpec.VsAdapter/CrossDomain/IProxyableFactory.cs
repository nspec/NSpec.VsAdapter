namespace NSpec.VsAdapter.CrossDomain
{
    public interface IProxyableFactory<TProxyable>
    {
        TProxyable CreateProxy(ITargetAppDomain targetDomain);
    }
}
