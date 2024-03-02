using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SelfcastSpell : Spell
    {
        public SelfcastSpell(SpellData data) : base(data)
        {
        }

        public override bool CanCast(Unit caster, Unit target, SpellValueProvider values) => caster == target;
        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(new(caster, target, Id));

            for (int i = 0; i < EffectsCount; i++)
            {
                record.AddAction(ApplyEffect(i, values.GetEffectBonus(i), caster, caster));
            }

            return record;
        }
    }
}