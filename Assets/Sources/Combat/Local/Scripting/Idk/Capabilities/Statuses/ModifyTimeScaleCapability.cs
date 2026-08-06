using Combat.API.Statuses;

namespace Combat.Local.Scripting.Idk.Capabilities.Statuses
{
    public readonly ref struct ModifyTimeScaleCapability
    {
        private readonly ITimeScaleModifier _modifier;

        public ModifyTimeScaleCapability(ITimeScaleModifier modifier)
        {
            _modifier = modifier;
        }

        public float GetModification() => _modifier.GetTimeModification();
    }
}
