using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.CastingBehaviors;
using Core.Combat.Engine;
using Core.Combat.Units.CastingBehaviors;
using Server.Combat.Core.Utils;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public sealed class Spellcasting : Updatable
    {
        private const int SPELL_LIST_SIZE = 16;
        private const int ACTIONBAR_SIZE = 6;
        private const int ITEMBAR_SIZE = 4;
        private const int SCHOOL_COUNT = 12;
        private const int AUTOATTACK_COUNT = 2;

        private readonly List<Ability> _abilities = new List<Ability>(SPELL_LIST_SIZE);
        private readonly List<Ability> _actionbar = new List<Ability>(ACTIONBAR_SIZE);
        private readonly List<Ability> _itembar = new List<Ability>(ITEMBAR_SIZE);
        private readonly List<Ability> _autoattack = new List<Ability>(AUTOATTACK_COUNT);

        private readonly Dictionary<SpellId, SpellModification> _spellModification = new Dictionary<SpellId, SpellModification>(SPELL_LIST_SIZE);
        private readonly InterruptData[] _interrupts = new InterruptData[SCHOOL_COUNT];

        private Unit _owner;

        private CastStateMachine _casting = new();
        private Duration _globalCooldown;

        public float GCD => _globalCooldown.Left;
        public float CastTime => _casting.CurrentState.Duration.Left;
        public float FullCastTime => _casting.CurrentState.Duration.FullTime;
        public Spell CastingSpell => _casting.CurrentState.Spell;

        internal Ability SetAbilityInSlot(Ability ability, SpellSlot slot)
        {
            Ability result = GetAbility(slot);

            switch ((byte) slot)
            {
                case <= (byte) SpellSlot.SIXTH:
                    _actionbar[(int) slot] = ability;
                    break;

                case <= (byte) SpellSlot.ITEM_2:
                    _itembar[(int) slot - (int) SpellSlot.ITEM_1] = ability;
                    break;

                case <= (byte) SpellSlot.ATTACK_OFF_HAND:
                    _autoattack[(int) slot - (int) SpellSlot.ATTACK_MAIN] = ability;
                    break;

                default:
                    return null;
            }

            return result;
        }

        internal bool GiveAbility(Spell spell)
        {
            if (FindAbility(spell) != null)
            {
                return false;
            }

            Ability ability = new Ability(spell);

            _abilities.Add(ability);
            _spellModification[spell.Id] = new SpellModification(spell);

            if (spell.Flags.HasFlag(SpellFlags.PASSIVE_SPELL))
            {
                spell.Cast(_owner, _owner, GetSpellValues(spell.Id));
            }

            GetBarForSpell(spell)?.Add(ability);

            return true;
        }

        internal bool RemoveAbility(Spell spell)
        {
            bool ownSpell = _abilities.RemoveAll((ability) => ability.IsDriving(spell)) > 0;

            if (ownSpell == false)
            {
                return false;
            }

            if (spell.Flags.HasFlag(SpellFlags.PASSIVE_SPELL))
            {
                _owner.RemoveStatus(spell.Id);
            }
            else
            {
                GetBarForSpell(spell)?.RemoveAll((ability) => ability.IsDriving(spell));
            }

            return true;
        }

        public bool HasAbility(Spell spell) => _abilities.Exists((ability) => ability.SpellEquals(spell));

        internal Ability FindAbility(Spell spell) => _abilities.Find((ability) => ability.IsDriving(spell));

        public Ability GetAbility(SpellSlot slot) => slot switch
        {
            <= SpellSlot.SIXTH => _actionbar.Count > (int) slot ? _actionbar[(int) slot] : null,
            SpellSlot.ITEM_1 => _itembar.Count > 0 ? _itembar[0] : null,
            SpellSlot.ITEM_2 => _itembar.Count > 1 ? _itembar[1] : null,
            SpellSlot.ATTACK_OFF_HAND => _autoattack.Count > 0 ? _autoattack[0] : null,
            SpellSlot.ATTACK_MAIN => _autoattack.Count > 1 ? _autoattack[1] : null,
            _ => null,
        };

        internal SpellValueProvider GetSpellValues(SpellId spell) => new(Spell.Get(spell), _spellModification.GetValueOrDefault(spell, new(Spell.Get(spell))));

        internal PrecastActionRecord StartCast(Ability ability, Unit target, float time)
        {
            Precast precast = new Precast(new(ability, _owner, target), time);
            _casting.ChangeState(precast);

            return new PrecastActionRecord(_owner.Id, target.Id, ability.Spell.Id, time);
        }

        public void Interrupt(InterruptData data)
        {
            if (data.Succes)
            {
                _casting.ChangeState(new Idle());
                return;
            }

            if (_casting.CurrentState.CanInterrupt == false)
            {
                return;
            }

            for (int i = 0; i < _interrupts.Length; i++)
            {
                if ((((int) _casting.CurrentState.Spell.School >> i) & 1) != 0)
                {
                    _interrupts[i] = data;
                }
            }
        }

        public void OverrideAbility(Spell replace, Spell with)
        {
            throw new NotImplementedException();
        }

        public void ModifyCooldown(SpellId spellId, PercentModifiedValue value) => GetModificationForSpell(spellId).Cooldown += value;

        public void ModifySpellEffect(SpellId spellId, int effect, float modification) => GetModificationForSpell(spellId).EffectsModifications[effect] += modification;

        public float GetCooldown(Spell spell)
        {
            Ability ability = FindAbility(spell);

            if (ability == null)
            {
                return 0;
            }

            return ability.CooldownTime;
        }

        public bool TryAssignTo(Unit unit)
        {
            if (_owner != null)
            {
                return false;
            }

            _owner = unit;
            return true;
        }

        public void Update(IActionRecordContainer container)
        {
            if (_owner.Alive == false)
            {
                return;
            }

            _casting.CurrentState.Update(container);

            if (_casting.CurrentState.Finished)
            {
                //TODO: container.AddAction(StopCastAction());
                _casting.ChangeState(new Idle());
            }

            if (_casting.CurrentState.AllowAutoattack && _owner.CanHurt(_owner.Target))
            {
                for (int i = 0; i < AUTOATTACK_COUNT; i++)
                {
                    if (i >= _autoattack.Count || _autoattack[i].OnCooldown)
                    {
                        continue;
                    }

                    container.AddAction(_autoattack[i].Cast(_owner, _owner.Target, GetSpellValues(_autoattack[i].Spell.Id)));
                }
            }
        }

        private SpellModification GetModificationForSpell(SpellId spellId)
        {
            SpellModification result;

            if (_spellModification.ContainsKey(spellId) == false)
            {
                result = new SpellModification(Spell.Get(spellId));
                _spellModification[spellId] = result;
            }
            else
            {
                result = _spellModification[spellId];
            }

            return result;
        }

        private List<Ability> GetBarForSpell(Spell spell)
        {
            if (spell.Flags.HasFlag(SpellFlags.AUTOATTACK))
            {
                return _autoattack;
            }
            else if (spell.Flags.HasFlag(SpellFlags.ITEM_PROVIDED_SPELL))
            {
                return _itembar;
            }
            else if (spell.Flags.HasFlag(SpellFlags.PASSIVE_SPELL))
            {
                return null;
            }
            else
            {
                return _actionbar;
            }
        }
    }
}