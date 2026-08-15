namespace Combat.Local.Scripting.IDK
{
    public interface IStatusCapabilityProvider
    {
        T GetCapability<T>() where T : class;
    }
}
