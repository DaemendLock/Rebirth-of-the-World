using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface IHitRecordQueue
    {
        void Enqueue(HitRecord record);
        bool TryDequeue(out HitRecord record);
    }

    public interface IHitboxOwnerRepository
    {
        void Create(UnitId id);
        void Delete(UnitId id);
    }
}
