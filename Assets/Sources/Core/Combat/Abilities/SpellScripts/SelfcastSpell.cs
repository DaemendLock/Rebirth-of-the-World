using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SelfcastSpell : Spell
    {
        public SelfcastSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(Unit data, SpellValueProvider values) => CommandResult.SUCCES;

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(caster, target, Id);

            for (int i = 0; i < EffectsCount; i++)
            {
                record.AddAction(ApplyEffect(i, values.EffectBonus(i), caster, caster));
            }

            return record;
        }
    }
}