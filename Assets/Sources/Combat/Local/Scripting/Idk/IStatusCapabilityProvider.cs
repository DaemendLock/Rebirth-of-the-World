namespace Combat.Local.Scripting.IDK
{
    public interface IStatusCapabilityProvider
    {
        bool TryGetCapability<T>(out T capability) where T : class;
    }
}
