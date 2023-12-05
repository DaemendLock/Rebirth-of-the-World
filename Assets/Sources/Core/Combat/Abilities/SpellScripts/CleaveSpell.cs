using Core.Combat.Units;
using Core.Combat.Utils;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellScripts
{
    public class CleaveSpell : Spell
    {
        public const int CLEAVE_EFFECT_INDEX = 0;

        public CleaveSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(CastEventData data, SpellModification modification)
        {
            return CommandResult.SUCCES;
        }

        public override void Cast(CastEventData data, SpellModification modification)
        {
            float angle = GetEffectValue(CLEAVE_EFFECT_INDEX, modification.EffectsModifications[CLEAVE_EFFECT_INDEX]);

            Unit caster = data.Caster;
            Team.Team ignorTeam = GetIgnorTeam(caster, TargetTeam);

            float effectiveCastRange = GetEffectiveRange(Range, modification);

            List<Unit> targets = Engine.Combat.FindUnitsInRadius(caster.Position, effectiveCastRange, Flags.HasFlag(SpellFlags.TARGET_DEAD), ignorTeam);

            if (angle >= 180)
            {
                foreach (Unit target in targets)
                {
                    for (int i = 1; i < EffectsCount; i++)
                    {
                        ApplyEffect(i, modification.EffectsModifications[i], new CastEventData(data.Caster, target, data.Spell));
                    }
                }

                return;
            }

            Vector3 casterPosition = caster.Position;
            Vector3 casterViewAngle = new(caster.Rotation);

            foreach (Unit target in targets)
            {
                if (Vector3.Angle(target.Position - casterPosition, casterViewAngle) > angle)
                {
                    continue;
                }

                for (int i = 1; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModifications[i], new CastEventData(data.Caster, target, data.Spell));
                }
            }
        }
    }
}
