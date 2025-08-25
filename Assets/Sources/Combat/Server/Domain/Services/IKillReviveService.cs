using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Services
{
    public interface IStatusApplicationService
    {
        StatusEffect AddStatus(StatusApplicationData data);
    }

    public interface IKillReviveService
    {
        void Kill(Unit target, KillData data);

        void Revive(Unit target, ReviveData data);
    }
}
