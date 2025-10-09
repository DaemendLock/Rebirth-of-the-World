using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public struct Aligment
    {
        public Aligment(EntityId id, Team team)
        {
            Id = id;
            Team = team;
        }

        public EntityId Id { get; }

        public Team Team { get; set; }
    }
}
