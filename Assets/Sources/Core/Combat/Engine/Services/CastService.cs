using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using System;
using Utils.ThrowHepler;

namespace Core.Combat.Engine.Services
{
    internal readonly struct CastData
    {
        public CastData(Ability ability, Unit caster, Unit target)
        {
            ThrowHepler.ArgumentNullException(ability, caster, target);

            Caster = caster;
            Target = target;
            Ability = ability;
        }

        public Unit Caster { get; }
        public Ability Ability { get; }
        public Unit Target { get; }
    }

    internal class CastService
    {
        private readonly Unit _caster;
        private readonly Ability _ability;
        private readonly Unit _target;
        private readonly SpellValueProvider _spellValues;

        internal CastService(Unit caster, Unit target, Ability ability)
        {
            _caster = caster ?? throw new ArgumentNullException(nameof(caster));
            _ability = ability;
            _target = target;
            _spellValues = _caster.GetSpellValues(ability.Spell.Id);
        }

        public CommandResult Check()
        {
            if (_caster.CanPay(_spellValues.Cost) == false)
            {
                return CommandResult.NOT_ENOUGHT_RESOURCE;
            }

            if ((_caster.Position - _target.Position).sqrMagnitude > (_spellValues.Range * _spellValues.Range))
            {
                return CommandResult.OUT_OF_RANGE;
            }

            return _ability.CanCast(_target, _spellValues);
        }

        public CastActionRecord Cast()
        {
            if (_spellValues.CastTime > 0)
            {
                CastActionRecord result = new(_caster, _target, _ability.Spell.Id);
                result.AddAction(_caster.StartCast(_ability, _target, _spellValues.CastTime));
                return result;
            }

            return _ability.Cast(_caster, _target, _spellValues);
        }
    }
}
