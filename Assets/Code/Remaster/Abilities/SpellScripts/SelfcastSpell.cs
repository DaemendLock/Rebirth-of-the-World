using Remaster.Events;

namespace Remaster.SpellScripts
{
    public class SelfcastSpell : Spell
    {
        public SelfcastSpell(SpellData data) : base(data)
        {
        }

        public override void Cast(EventData data, SpellModification modification)
        {
            data = new EventData(data.Caster, data.Caster, data.Spell);

            base.Cast(data, modification);
        }
    }
}