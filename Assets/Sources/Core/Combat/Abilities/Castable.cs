using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
namespace Core.Combat.Abilities
{
    public interface Castable
    {
        CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values);

        CommandResult CanCast(Unit data, SpellValueProvider values);
    }
}
