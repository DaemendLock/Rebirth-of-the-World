using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IAligmentRepository
    {
        Aligment Get(UnitId id);
        void Create(Aligment value);
        void Update(Aligment value);
        void Delete(UnitId id);
    }
}
