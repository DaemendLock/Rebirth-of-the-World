using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface IAbilityApiAdapter
    {
        IAbilityApi Adaptee(AbilityKey abilityKey);
    }
}