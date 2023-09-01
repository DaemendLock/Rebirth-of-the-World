using Core.Engine;
using Core.Combat.Abilities;
using Core.Combat.CastingBehaviors;
using Core.Combat.Utils;
using System;
using System.Collections.Generic;
using Utils;
using Core.Combat.Interfaces;

namespace Core.Combat.Units.Components
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
            Ability ability = FindAbility(data.Spell);
            
            if(ability == null)
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

        public CommandResult CastSpell(EventData data)
        {
            Spell spell = data.Spell;
            Unit target = data.Target;

            SpellModification spellModification = GetModificationForSpell(spell.Id);

            target = DecideTarget(target, spell, spellModification);

            if (target == null)
            {
                return CommandResult.INVALID_TARGET;
            }

            if ((spell.GcdCategory == GcdCategory.NORMAL) && (_globalCooldown.Expired == false))
            {
                return CommandResult.ON_COOLDOWN;
            }

            data = new EventData(_owner, target, spell, data.TriggerTime);
            CommandResult result = spell.CanCast(data, spellModification);

            if (result != CommandResult.SUCCES)
            {
                return result;
            }

            StartCast(data, spellModification);
            return CommandResult.SUCCES;
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

        private Unit DecideTarget(Unit target, Spell spell, SpellModification modification)
        {
            if (_owner != null && spell.TargetTeam == TargetTeam.ALLY && (_owner.CanHelp(target) == false ||
                IsSelfCasting(spell, modification)))
            {
                return _owner;
            }

            if (spell.TargetTeam == TargetTeam.ENEMY && (_owner.CanHurt(_owner) == false))
            {
                return null;
            }

            return _owner;
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
                result = new SpellModification(Data.SpellLib.SpellLib.GetSpell(spellId));
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