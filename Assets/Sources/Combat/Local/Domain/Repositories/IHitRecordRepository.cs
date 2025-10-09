using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHitRecordRepository
    {
        void Register(HitRecord value);
        bool TryPop(out HitRecord result);
    }
}
