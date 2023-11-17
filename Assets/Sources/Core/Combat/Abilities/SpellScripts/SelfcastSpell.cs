using Core.Combat.Utils;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SelfcastSpell : Spell
    {
        public SelfcastSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(CastEventData data, SpellModification modification)
        {
            return CommandResult.SUCCES;
        }

        public override void Cast(CastEventData data, SpellModification modification)
        {
            data = new CastEventData(data.Caster, data.Caster, data.Spell);

            base.Cast(data, modification);
        }
    }
}