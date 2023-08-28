using Remaster.Abilities;
using Remaster.CombatSetup;
using Remaster.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Remaster.Test
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private int[] _spellId;
        [SerializeField] private UnitData _unitData;
        [SerializeField] private Team _team;
        [SerializeField] private KeyCode _checkButton;

        private Remaster.Unit _unit;

        private void Awake()
        {
            List<Spell> spells = new List<Spell>();

            foreach (int id in _spellId)
            {
                spells.Add(SpellLib.SpellLib.GetSpell(id));
            }

            _unitData.BasicSpells = spells.ToArray();

            _unit = UnitFactory.CreateUnit(_unitData, _team, new Utils.Position());
        }

        private void Update()
        {
            _slider.value = _unit.CurrentHealth / _unit.MaxHealth;

            AutoAttack();

            if (Input.GetKeyDown(_checkButton))
            {
                Spell spell = _unit.GetAbility(SpellSlot.FIRST).Spell;

                Remaster.Unit target = Engine.Combat.GetTargetFor(_unit, spell);

                _unit.CastAbility(new(_unit, target, spell));
            }
        }

        private void AutoAttack()
        {
            Ability ability = _unit.GetAbility(SpellSlot.ATTACK_MAIN);

            if (ability != null && ability.Cooldown.Expired)
            {
                Remaster.Unit target = Engine.Combat.GetTargetFor(_unit, ability.Spell);

                _unit.CastAbility(new(_unit, target, ability.Spell));
            }

            ability = _unit.GetAbility(SpellSlot.ATTACK_OFF_HAND);

            if (ability != null && ability.Cooldown.Expired)
            {
                Remaster.Unit target = Engine.Combat.GetTargetFor(_unit, ability.Spell);

                _unit.CastAbility(new(_unit, target, ability.Spell));
            }
        }
    }
}

