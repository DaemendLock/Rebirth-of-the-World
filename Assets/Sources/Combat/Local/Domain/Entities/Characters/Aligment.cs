using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public ref struct Aligment
    {
        public Aligment(UnitId id, Team team)
        {
            Id = id;
            Team = team;
        }

        public UnitId Id { get; }

        public Team Team { get; set; }
    }
}
