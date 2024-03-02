using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using System;
using System.Runtime.CompilerServices;
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
        private Unit _target;
        private readonly SpellValueProvider _spellValues;

        internal CastService(Unit caster, Unit target, Ability ability)
        {
            _caster = caster ?? throw new ArgumentNullException(nameof(caster));
            _ability = ability;
            _target = target;
            _spellValues = _caster.GetSpellValues(ability.Spell.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckCast()
        {
            if (_ability.CanCast(_caster, _target, _spellValues))
            {
                return true;
            }

            _target = _caster;

            return _ability.CanCast(_caster, _target, _spellValues);
        }

        public CastActionRecord Cast()
        {
            if (_spellValues.CastTime > 0)
            {
                CastActionRecord result = new(new(_caster, _target, _ability.Spell.Id));
                result.AddAction(_caster.StartCast(_ability, _target, _spellValues.CastTime));
                return result;
            }

            return _ability.Cast(_caster, _target, _spellValues);
        }
    }
}
