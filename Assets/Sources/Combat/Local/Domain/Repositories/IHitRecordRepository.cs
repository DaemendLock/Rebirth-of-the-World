using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface ICastQueueRepository
    {
        void Enqueue(CastInfo info);
        bool TryPop(out CastInfo result);
    }

    public interface IHitRecordRepository
    {
        void Register(HitRecord value);
        bool TryPop(out HitRecord result);
    }
}
