using Core.Combat.Units;
using Core.Combat.Utils;
using System.Collections.Generic;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SplashSpell : Spell
    {
        public SplashSpell(SpellData data) : base(data)
        {
        }

        public override CommandResult CanCast(CastEventData data, SpellModification modification) => CommandResult.SUCCES;

        public override void Cast(CastEventData data, SpellModification modification)
        {
            float effectiveCastRange = GetEffectiveRange(Range, modification);
            Unit caster = data.Caster;
            Team.Team ignorTeam = GetIgnorTeam(caster, TargetTeam);

            List<Unit> targets = Engine.Combat.FindUnitsInRadius(caster.Position, effectiveCastRange, Flags.HasFlag(SpellFlags.TARGET_DEAD) == false, Team.Team.NONE);

            foreach (Unit target in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModifications[i], new CastEventData(caster, target, data.Spell));
                }
            }
        }
    }
}