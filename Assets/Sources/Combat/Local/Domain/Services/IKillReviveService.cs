using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Services
{
    public interface IKillReviveService
    {
        bool IsAlive(EntityId target);

        void Kill(EntityId target);

        void Revive(EntityId target);
    }
}
