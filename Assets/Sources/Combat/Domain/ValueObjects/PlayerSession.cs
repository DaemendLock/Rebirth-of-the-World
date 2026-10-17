using Combat.Common.Primitives;

namespace Combat.Domain.ValueObjects
{
    public sealed class PlayerSession
    {
        public UnitId? ControlledUnitId { get; set; }
    }
}
