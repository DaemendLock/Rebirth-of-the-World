using Remaster.Abilities;
using Remaster.CastingBehaviors;
using Remaster.Combat;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Utils;
using System;
using System.Collections.Generic;

namespace Remaster.UnitComponents
{
    public class Spellcasting : AbilityOwner, Updatable
    {
        private const int SPELL_LIST_SIZE = 16;
        private const int ACTIONBAR_LIST_SIZE = 6;
        private const int ITEMBAR_LIST_SIZE = 4;
        private const int SCHOOL_COUNT = 12;

        private readonly List<Ability> _abilities = new List<Ability>(SPELL_LIST_SIZE);
        private readonly List<Ability> _actionbar = new List<Ability>(ACTIONBAR_LIST_SIZE);
        private readonly List<Ability> _itembar = new List<Ability>(ITEMBAR_LIST_SIZE);
        private readonly List<Ability> _autoattack = new List<Ability>(2);

        private readonly Dictionary<int, SpellModification> _spellModification = new Dictionary<int, SpellModification>(SPELL_LIST_SIZE);
        private readonly InterruptData[] _interrupts = new InterruptData[SCHOOL_COUNT];

        private Unit _owner;

        private CastingBehavior _casting = new Idle();
        private Duration _globalCooldown;

        public IReadOnlyList<IReadOnlyAbility> Actionbar => _actionbar;
        public IReadOnlyList<IReadOnlyAbility> Itembar => _itembar;

        public bool GiveAbility(Spell spell)
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
                CastAbility(new EventData(_owner, _owner, spell));
            }

            GetBarForSpell(spell)?.Add(ability);

            return true;
        }

        public bool RemoveAbility(Spell spell)
        {
            bool ownSpell = _abilities.RemoveAll((ability) => ability.IsDriving(spell)) > 0;

            if (ownSpell == false)
            {
                return false;
            }

            GetBarForSpell(spell)?.RemoveAll((ability) => ability.IsDriving(spell));

            return true;
        }

        public bool HasAbility(Spell spell) => _abilities.Exists((ability) => ability.SpellEquals(spell));

        public Ability FindAbility(Spell spell) => _abilities.Find((ability) => ability.IsDriving(spell));

        public Ability GetAbility(SpellSlot slot) => slot switch
        {
            <= SpellSlot.SIXTH => _actionbar.Count > (int) slot ? _actionbar[(int) slot] : null,
            SpellSlot.ITEM_1 => _itembar.Count > 0 ? _itembar[0] : null,
            SpellSlot.ITEM_2 => _itembar.Count > 1 ? _itembar[1] : null,
            SpellSlot.ATTACK_OFF_HAND => _autoattack.Count > 0 ? _autoattack[0] : null,
            SpellSlot.ATTACK_MAIN => _autoattack.Count > 1 ? _autoattack[1] : null,
            _ => null,
        };

        public CommandResult CastAbility(EventData data)
        {
            Spell spell = data.Spell;
            Ability ability = FindAbility(data.Spell);
            SpellModification spellModification = GetModificationForSpell(spell.Id);

            Unit target = DecideTarget(data, spellModification);

            if (target == null)
            {
                Logger.LogCastFailed(data, CommandResult.INVALID_TARGET);
                return CommandResult.INVALID_TARGET;
            }

            if ((spell.GcdCategory == GcdCategory.NORMAL) && (_globalCooldown.Expired == false))
            {
                Logger.LogCastFailed(data, CommandResult.ON_COOLDOWN);
                return CommandResult.ON_COOLDOWN;
            }

            EventData castData = new EventData(data.Caster, target, spell);

            CommandResult result = ability?.CanCast(castData, spellModification) ?? CommandResult.SUCCES;

            if (result == CommandResult.SUCCES)
            {
                StartCast(castData, spellModification);
                return CommandResult.SUCCES;
            }

            Logger.LogCastFailed(data, result);
            return result;
        }

        public void Channel(EventData data, float triggerInterval)
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
                if ((((int)_casting.School >> i) & 1) != 0)
                {
                    _interrupts[i] = data;
                }
            }
        }

        public void OverrideAbility(Spell replace, Spell with)
        {
            throw new NotImplementedException();
        }

        public void ReduceCooldown(int spellId, PercentModifiedValue value) => GetModificationForSpell(spellId).CooldownReduction += value;

        public void ModifySpellEffect(int spellId, int effect, float modification) => GetModificationForSpell(spellId).EffectsModificationList[effect] += modification;

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
            _casting.OnUpdate();

            if (_casting.Finished)
            {
                CastingBehavior active = _casting;
                _casting = new Idle();
                _casting.OnCastEnd();
            }
        }

        private void StartCast(EventData data, SpellModification modification)
        {
            if (data.Spell.CastTime + modification.BonusCastTime.CalculatedValue == 0)
            {
                Castable ability = FindAbility(data.Spell);
                ability ??= data.Spell;

                ability.Cast(data, modification);
            }
            else
            {
                _casting = new Precast(data, modification);
                _casting.OnCastBegins();
            }

            StartGcd(data.Spell, data.Caster);
        }

        private Unit DecideTarget(EventData data, SpellModification modification)
        {
            if (data.Caster != null && data.Spell.TargetTeam == TargetTeam.ALLY && (data.Caster.CanHelp(data.Target) == false ||
                IsSelfCasting(data.Spell, modification)))
            {
                return data.Caster;
            }

            if (data.Spell.TargetTeam == TargetTeam.ENEMY && (data.Caster.CanHurt(data.Target) == false))
            {
                return null;
            }

            return data.Target;
        }

        private bool IsSelfCasting(Spell spell, SpellModification spellModification) => (new PercentModifiedValue(spell.Range, 100) + spellModification.BonusRange).CalculatedValue <= 0;

        private void StartGcd(Spell spell, Unit caster)
        {
            if (spell.GcdCategory != GcdCategory.NORMAL)
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

        private SpellModification GetModificationForSpell(int spellId)
        {
            SpellModification result;

            if (_spellModification.ContainsKey(spellId) == false)
            {
                result = new SpellModification(SpellLib.SpellLib.GetSpell(spellId));
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