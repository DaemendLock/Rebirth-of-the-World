using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SplashSpell : Spell
    {
        private const byte AOE_RADIUS_INDEX = 0;

        public SplashSpell(SpellData data) : base(data)
        {
        }

        //public override void Cast(CastInputData data)
        //{
        //    Unit[] targets = new Unit[0];
        //    //targets = GetUnitsInRadius(range, team, data.Target.Position)

        //    foreach (Unit target in targets)
        //    {
        //        for (int i = 0; i < EffectsCount; i++)
        //        {
        //            ApplyEffect(i, modification.EffectsModifications[i], new CastEventData(data.Caster, target, data.Spell, data.TriggerTime));
        //        }
        //    }
        //}

        //public override CommandResult CanCast(CastInputData data, SpellModification modification)
        //{
        //    Unit caster = data.Caster;
        //    Unit target = data.Target;

        //    if (target == null)
        //    {
        //        return CommandResult.INVALID_TARGET;
        //    }

        //    float effectiveCastRange = Range * (1 + modification.BonusRange.Percent / 100) + modification.BonusRange.BaseValue;

        //    if ((caster.Position - target.Position).sqrMagnitude > effectiveCastRange * effectiveCastRange)
        //    {
        //        return CommandResult.OUT_OF_RANGE;
        //    }

        //    if (TargetTeam == TargetTeam.Ally && (!caster.CanHelp(target)))
        //    {
        //        return CommandResult.INVALID_TARGET;
        //    }

        //    if (TargetTeam == TargetTeam.Enemy && (!caster.CanHurt(target)))
        //    {
        //        return CommandResult.INVALID_TARGET;
        //    }

        //    return CommandResult.SUCCES;
        //}

        public override CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            throw new System.NotImplementedException();
        }

        public override CommandResult CanCast(Unit data, SpellValueProvider values)
        {
            throw new System.NotImplementedException();
        }
    }
}

