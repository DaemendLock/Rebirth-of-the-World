using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IMoveOverTimeEffectRepository
    {
        void Create(MoveInDirectionEffect value);
        void Update(MoveInDirectionEffect value);
        MoveInDirectionEffect Get(TransformEffectId id);
        void Delete(TransformEffectId id);
    }

    public interface IStateRepository
    {
        CharacterState Get(EntityId id);
        void Create(CharacterState killable);
        void Update(CharacterState killable);
        void Delete(EntityId id);
    }
}
