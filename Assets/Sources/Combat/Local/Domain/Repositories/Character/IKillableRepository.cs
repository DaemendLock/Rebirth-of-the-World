using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IKillableRepository
    {
        Killable Get(EntityId id);
        void Create(Killable killable);
        void Update(Killable killable);
        void Delete(EntityId id);
    }
}
