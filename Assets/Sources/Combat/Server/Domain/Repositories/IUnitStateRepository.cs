using System.Collections.Generic;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.ValueObjects.Statuses;
using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Domain.Repositories
{
    public interface ITransformRepository
    {
        void Add(EntityId id, Transform transform);
        Transform Get(EntityId id);
        void Update(EntityId id, Transform transform);
        void Remove(EntityId id);
    }

    public interface IKillableRepository
    {
        void Add(Unit unit);
        Unit Get(EntityId id);
        void Remove(EntityId id);
    }

    public interface IStatusRepository
    {
        IEnumerable<StatusEffect> FindStatusEffects(EntityId id);
        StatusEffect Get(StatusId statusId);
        void Add(StatusEffect statusEffect);
        void Remove(StatusId id);
    }
}
