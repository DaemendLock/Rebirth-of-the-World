using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IAligmentRepository
    {
        Aligment Get(EntityId id);
        void Create(Aligment value);
        void Update(Aligment value);
        void Delete(EntityId id);
    }
}
