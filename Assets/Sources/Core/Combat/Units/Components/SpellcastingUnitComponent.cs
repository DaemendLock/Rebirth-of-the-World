using Core.Combat.Abilities;
using Core.Combat.CastingBehaviors;
using Core.Combat.Engine;
using Core.Combat.Utils;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class Spellcasting : Updatable
    {
        public delegate void StartedPrecast(Spell spell, float duration);
        public delegate void Casted(Spell spell, CommandResult result);
        public delegate void Attacked(Spell spell, Unit target);
        public delegate void StoppedPrecast();

        public delegate void CastStarted(Spell spell);

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

        private CastingBehavior _casting = new Idle();
        private Duration _globalCooldown;

        public float GCD => _globalCooldown.Left;
        public float CastTime => _casting.TimeLeft;
        public float FullCastTime => _casting.FullTime;
        public Spell CastingSpell => _casting.Spell;

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
                CastSpell(new CastEventData(_owner, _owner, spell));
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
                _owner.RemoveStatus(spell);
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

        internal CommandResult CastAbility(CastEventData data)
        {
            Ability ability = FindAbility(data.Spell);

            if (ability == null)
            {
                return CommandResult.INVALID_TARGET;
            }

            if (ability.OnCooldown)
            {
                return CommandResult.ON_COOLDOWN;
            }

            if (ability.CanPay(data, GetModificationForSpell(ability.Spell.Id)) == false)
            {
                return CommandResult.NOT_ENOUGHT_RESOURCE;
            }

            return CastSpell(data);
        }

        internal CommandResult CastSpell(CastEventData data)
        {
            Spell spell = data.Spell;
            Unit target = data.Target;

            SpellModification spellModification = GetModificationForSpell(spell.Id);

            if (data.Spell.Flags.HasFlag(SpellFlags.PROC_SPELL))
            {
                data.Spell.Cast(data, spellModification);
                return CommandResult.SUCCES;
            }

            target = DecideTarget(target, spell, spellModification);

            if ((spell.GcdCategory == GcdCategory.Normal) && (_globalCooldown.Expired == false))
            {
                return CommandResult.ON_COOLDOWN;
            }

            /*
            if (spell.Flags.HasFlag(SpellFlags.CAN_CAST_WHILE_MOVING) || (_owner.MoveDirection != Vector3.zero))
            {
                return CommandResult.MOVING;
            }*/

            data = new CastEventData(data.Caster, target, spell, data.TriggerTime);
            CommandResult result = spell.CanCast(data, spellModification);

            if (result != CommandResult.SUCCES)
            {
                return result;
            }

            StartCast(data, spellModification);
            return CommandResult.SUCCES;
        }

        public void Channel(CastEventData data, float triggerInterval)
        {
            _casting = new Channeling(data, GetModificationForSpell(data.Spell.Id), triggerInterval);
        }

        public void Interrupt(InterruptData data)
        {
            if (data.Succes)
            {
                _casting = new Idle();
                return;
            }

            if (_casting.CanInterrupt == false)
            {
                return;
            }

            for (int i = 0; i < _interrupts.Length; i++)
            {
                if ((((int) _casting.School >> i) & 1) != 0)
                {
                    _interrupts[i] = data;
                }
            }
        }

        public void OverrideAbility(Spell replace, Spell with)
        {
            throw new NotImplementedException();
        }

        public void ReduceCooldown(SpellId spellId, PercentModifiedValue value) => GetModificationForSpell(spellId).CooldownReduction += value;

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

        public void Update()
        {
            if(_owner.Alive == false)
            {
                return;
            }

            _casting.OnUpdate();

            if (_casting.Finished)
            {
                CastingBehavior active = _casting;
                _casting = new Idle();
                active.OnCastEnd();
                _owner.InformCastingStopped();
            }

            if (_casting.AllowAutoattack && _owner.CanHurt(_owner.Target))
            {
                for (int i = 0; i < AUTOATTACK_COUNT; i++)
                {
                    if (i >= _autoattack.Count || _autoattack[i].OnCooldown)
                    {
                        continue;
                    }

                    _owner.Attack(_autoattack[i].Spell, _owner.Target);
                }
            }
        }

        private void StartCast(CastEventData data, SpellModification modification)
        {
            if (_casting.AllowAutoattack && (data.Spell.Flags.HasFlag(SpellFlags.CAN_CAST_WHILE_CASTING) == false))
            {
                _owner.Interrupt(new(true, new SpellId(), 0));
            }

            if (data.Spell.CastTime + modification.BonusCastTime.CalculatedValue == 0)
            {
                Castable ability = FindAbility(data.Spell);
                ability ??= data.Spell;

                ability.Cast(data, modification);
            }
            else
            {
                _casting = new Precast(data, modification);
                _owner.InformCastingBehavior(data.Spell, _casting.FullTime);
            }

            StartGcd(data.Spell, data.Caster);
        }

        private Unit DecideTarget(Unit target, Spell spell, SpellModification modification)
        {
            if (_owner != null && spell.TargetTeam == TargetTeam.Ally && (_owner.CanHelp(target) == false ||
                IsSelfCasting(spell, modification)))
            {
                return _owner;
            }

            if (spell.TargetTeam == TargetTeam.Enemy && (_owner.CanHurt(target) == false))
            {
                return null;
            }

            return target;
        }

        private bool IsSelfCasting(Spell spell, SpellModification spellModification) => (new PercentModifiedValue(spell.Range, 100) + spellModification.BonusRange).CalculatedValue <= 0;

        private void StartGcd(Spell spell, Unit caster)
        {
            if (spell.GcdCategory != GcdCategory.Normal)
            {
                return;
            }

            if (caster != null)
            {
                _globalCooldown = new Duration(spell.GCD / caster.EvaluateHasteTimeDivider());
            }
            else
            {
                _globalCooldown = new Duration(spell.GCD);
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