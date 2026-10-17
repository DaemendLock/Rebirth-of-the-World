using Combat.Common.Primitives;

namespace Combat.Domain.Repositories
{
    public interface IHurtableRepository
    {
        void Create(UnitId owner);
        void Delete(UnitId id);
    }
}
