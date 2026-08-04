using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface IAbilityApiAdapter
    {
        AbilityApi Adaptee(AbilityKey abilityKey);
    }
}