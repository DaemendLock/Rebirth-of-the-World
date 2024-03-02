using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using System.Collections.Generic;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SplashSpell : Spell
    {
        private const byte AOE_RADIUS_INDEX = 0;

        public SplashSpell(SpellData data) : base(data)
        {
        }

        public override bool CanCast(Unit caster, Unit target, SpellValueProvider values) =>
            CanTarget(caster, target, TargetTeam) && CanReach(caster, target, values.Range);

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(new(caster, target, Id));

            IEnumerable<Unit> targets = Engine.Units.FindUnitsInRadius(target.Position, GetEffectValue(AOE_RADIUS_INDEX, values.GetEffectBonus(AOE_RADIUS_INDEX)), GetSearchTeam(caster, TargetTeam), Flags.HasFlag(SpellFlags.TARGET_DEAD));

            foreach (Unit applicationTarget in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    record.AddAction(ApplyEffect(i, values.GetEffectBonus(i), caster, applicationTarget));
                }
            }

            return record;
        }
    }
}

