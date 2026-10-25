using Combat.API.Statuses;
using Combat.Local.Application.ValueObjects;

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

    public interface IStatusModifyMovespeedCapability
    {
        AttributeModifcation GetModification();
    }

    public sealed class ModifyMovespeedCapability : IStatusModifyMovespeedCapability
    {
        private readonly IMovespeedModifier _modifier;

        public ModifyMovespeedCapability(IMovespeedModifier modifier)
        {
            _modifier = modifier;
        }

        public AttributeModifcation GetModification() =>
            new(_modifier.GetBonusMovespeedValue(), _modifier.GetBonusMovespeedPercent(), _modifier.GetBonusMovespeed());
    }
}
