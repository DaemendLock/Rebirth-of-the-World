using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IStateRepository
    {
        CharacterState Get(EntityId id);
        void Create(CharacterState killable);
        void Update(CharacterState killable);
        void Delete(EntityId id);
    }
}
