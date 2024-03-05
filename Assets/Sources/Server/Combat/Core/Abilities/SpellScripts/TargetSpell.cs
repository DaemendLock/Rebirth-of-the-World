﻿using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;

namespace Core.Combat.Abilities.SpellScripts
{
    public class TargetSpell : Spell
    {
        public TargetSpell(SpellData data) : base(data)
        {
        }

        public override bool CanCast(Unit caster, Unit target, SpellValueProvider values) =>
            CanTarget(caster, target, TargetTeam) && CanReach(caster, target, values.Range);

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
            CastActionRecord record = new(new(caster, target, Id));

            for (int i = 0; i < EffectsCount; i++)
            {
                record.AddAction(ApplyEffect(i, values.GetEffectBonus(i), caster, target));
            }

            return record;
        }
    }
}