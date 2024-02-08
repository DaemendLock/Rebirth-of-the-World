using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using System.Collections.Generic;

namespace Core.Combat.Abilities.SpellScripts
{
    public class AoeSpell : Spell
    {
        public AoeSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(Unit data, SpellValueProvider values) => CommandResult.SUCCES;

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(caster, target, Id);

            float effectiveCastRange = values.Range;
            Team.Team team = GetSearchTeam(caster, TargetTeam);
            List<Unit> targets = Engine.Units.FindUnitsInRadius(caster.Position, effectiveCastRange, team, Flags.HasFlag(SpellFlags.TARGET_DEAD));

            foreach (Unit applicationTarget in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    record.AddAction(ApplyEffect(i, values.EffectBonus(i), caster, applicationTarget));
                }
            }

            return record;
        }
    }
}