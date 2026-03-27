using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IMovementEffectRepository
    {
        void Create(MoveInDirectionEffect value);
        void Delete(EntityId target, TransformEffectId id);
    }
}
