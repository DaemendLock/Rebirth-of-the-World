using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellScripts
{
    public class CleaveSpell : Spell
    {
        public const int CLEAVE_EFFECT_INDEX = 0;

        public CleaveSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(Unit data, SpellValueProvider values)
        {
            return CommandResult.SUCCES;
        }

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            CastActionRecord record = new(caster, target, Id);

            float effectiveCastRange = values.Range;
            float angle = GetEffectValue(CLEAVE_EFFECT_INDEX, values.EffectBonus(CLEAVE_EFFECT_INDEX));

            Team.Team ignorTeam = GetSearchTeam(caster, TargetTeam);
            List<Unit> targets = Engine.Units.FindUnitsInRadius(caster.Position, effectiveCastRange, ignorTeam, Flags.HasFlag(SpellFlags.TARGET_DEAD));

            if (angle >= 180)
            {
                foreach (Unit applicationTarget in targets)
                {
                    for (int i = 1; i < EffectsCount; i++)
                    {
                        record.AddAction(ApplyEffect(i, values.EffectBonus(i), caster, applicationTarget));
                    }
                }

                return record;
            }

            Vector3 casterPosition = caster.Position;
            Vector3 casterViewAngle = new(caster.Rotation);

            foreach (Unit applicationTarget in targets)
            {
                if (Vector3.Angle(target.Position - casterPosition, casterViewAngle) > angle)
                {
                    continue;
                }

                for (int i = 1; i < EffectsCount; i++)
                {
                    record.AddAction(ApplyEffect(i, values.EffectBonus(i), caster, applicationTarget));
                }
            }

            return record;
        }
    }
}
