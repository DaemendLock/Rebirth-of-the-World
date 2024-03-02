using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using System.Collections.Generic;

namespace Core.Combat.Abilities.SpellScripts
{
    public class AoeSpell : Spell
    {
        public AoeSpell(SpellData data) : base(data)
        {
        }

        public override bool CanCast(Unit caster, Unit target, SpellValueProvider values) => true;

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(new(caster, target, Id));

            float effectiveCastRange = values.Range;
            Team.Team team = GetSearchTeam(caster, TargetTeam);
            IEnumerable<Unit> targets = Engine.Units.FindUnitsInRadius(caster.Position, effectiveCastRange, team, Flags.HasFlag(SpellFlags.TARGET_DEAD));

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