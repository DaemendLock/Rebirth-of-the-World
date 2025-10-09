using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHurtableRepository
    {
        void Create(Hurtbox hitbox);
        void Delete(HurtboxId id);
        Hurtbox Get(HurtboxId id);
    }
}
