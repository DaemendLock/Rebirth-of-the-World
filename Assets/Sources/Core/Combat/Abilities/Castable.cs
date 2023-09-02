using Core.Combat.Utils;

namespace Core.Combat.Abilities
{
    public interface Castable
    {
        void Cast(EventData data, SpellModification modification);
        CommandResult CanCast(EventData data, SpellModification modification);
    }
}
