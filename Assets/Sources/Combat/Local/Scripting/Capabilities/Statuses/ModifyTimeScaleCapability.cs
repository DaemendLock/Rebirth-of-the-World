using Combat.API.Statuses;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusModifyTimeScaleCapability
    {
        float GetModification();
    }

    public sealed class ModifyTimeScaleCapability : IStatusModifyTimeScaleCapability
    {
        private readonly ITimeScaleModifier _modifier;

        public ModifyTimeScaleCapability(ITimeScaleModifier modifier)
        {
            _modifier = modifier;
        }

        public float GetModification() => _modifier.GetTimeModification();
    }
}
