namespace Combat.API.Scripting
{
    public interface IStatusScriptNew
    {
        void OnApply(IActor parent, Contexts.IStatusContext statusContext) { }
        void OnExpire(IActor parent, Contexts.IStatusContext statusContext) { }
        void OnRemove(IActor parent, Contexts.IStatusContext statusContext) { }
    }

    public interface IStatusTickableNew : IStatusScriptNew
    {
        void OnTick(IActor parent, Contexts.IStatusContext statusContext);
    }
}
