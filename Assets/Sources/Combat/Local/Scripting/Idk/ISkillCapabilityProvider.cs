namespace Combat.Local.Scripting.IDK
{
    public interface ISkillCapabilityProvider
    {
        T GetCapability<T>() where T : class;
    }
}
