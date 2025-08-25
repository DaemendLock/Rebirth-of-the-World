using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Infrastructure.Factories
{
    public interface IStatusFactory
    {
        public StatusEffect Create(StatusApplicationData data);
    }
}
