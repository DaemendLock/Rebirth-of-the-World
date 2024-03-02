using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;

namespace Core.Combat.Abilities
{
    public interface Castable
    {
        CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values);

        bool CanCast(Unit caster, Unit target, SpellValueProvider values);
    }
}
