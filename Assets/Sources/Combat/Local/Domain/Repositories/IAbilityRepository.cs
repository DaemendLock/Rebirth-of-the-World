using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IAbilityRepository
    {
        void Create(Ability value);

        Ability Get(AbilityKey key);

        void Update(Ability value);

        void Delete(AbilityKey key);

        IAbilityPropertyContainer GetPropertyContainer(AbilityKey key);
    }
}
