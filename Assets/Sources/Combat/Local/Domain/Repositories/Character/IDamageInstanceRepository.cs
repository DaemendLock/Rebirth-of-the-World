using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface IDamageEventsQueue
    {
        int Count { get; }

        void Enqueue(DamageResult instance);
        DamageResult Dequeue();
    }

    public interface IDamageInstanceQueue
    {
        int Count { get; }

        void Enqueue(DamageInstance instance);
        DamageInstance Dequeue();
    }
}
