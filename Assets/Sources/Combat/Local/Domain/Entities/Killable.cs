using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public struct Killable
    {
        public Killable(EntityId id, bool alive)
        {
            Id = id;
            Alive = alive;
        }

        public EntityId Id { get; }

        public bool Alive { get; set; }
    }
}
