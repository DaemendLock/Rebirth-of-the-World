using DaeHitbox;

using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Domain.Repositories
{
    public interface IHitboxesRepository
    {
        public void Add(EntityId entityId, IHitboxCollection hitboxCollection);

        public IHitboxCollection Get(EntityId entityId);

        public bool Remove(EntityId entityId);
    }
}
