using Combat.Common.Primitives;

namespace Combat.Local.Gateways.Models
{
    public readonly struct PlayerModel
    {
        public readonly UnitId? ControlledTarget;

        public PlayerModel(UnitId? controlledTarget)
        {
            ControlledTarget = controlledTarget;
        }
    }
}
