using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Player
    {
        public Player(PlayerId playerId, UnitId? controlledCharacter = default)
        {
            Id = playerId;
            ControlledEntity = controlledCharacter;
        }

        public readonly PlayerId Id { get; }

        public UnitId? ControlledEntity { get; set; }
    }
}
