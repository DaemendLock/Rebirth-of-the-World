using Core.Combat.Utils;

namespace Core.Combat.Abilities
{
    public interface Castable
    {
        void Cast(CastEventData data, SpellModification modification);
        CommandResult CanCast(CastEventData data, SpellModification modification);
    }
}
