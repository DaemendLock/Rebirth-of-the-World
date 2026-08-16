using Combat.Common.Primitives;

namespace Combat.Local.Domain.Services
{
    public sealed class PlayerSession
    {
        public UnitId? ControlledUnitId { get; set; }
        public bool AcceptsInputs { get; set; }
    }
}
