using Core.Combat.Abilities;
using Data.Characters;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataStructure;
using static Utils.DataTypes.UnitCreationData;
using CUnit = Core.Combat.Units.Unit;

namespace View.Combat.Units
{
    public class Unit : MonoBehaviour
    {
        private static Dictionary<int, Unit> _units = new();

        private CUnit _assignedUnit;
        private int _id;
        private Sprite[] _spellIcons;

        [SerializeField] private Animator _animator;

        private void Update()
        {
            Utils.DataTypes.Vector3 pos = _assignedUnit.Position;
            transform.position = new(pos.x, pos.y, pos.z);

            Utils.DataTypes.Vector3 rotation = new(_assignedUnit.Rotation);

            transform.forward = new Vector3(rotation.x, rotation.y, rotation.z);

            _animator.SetFloat("Movespeed", _assignedUnit.IsMoving ? _assignedUnit.GetStat(UnitStat.SPEED) : 0);
        }

        private void OnDestroy()
        {
            if (_assignedUnit == null)
            {
                return;
            }

            _units.Remove(_id);
        }

        public void Init(int id, ViewData data)
        {
            if (_assignedUnit != null)
            {
                return;
            }

            _assignedUnit = Core.Combat.Engine.Combat.GetUnit(id);
            _id = id;
            _spellIcons = Character.Get(data.CharacterId).GetSpellIcons(data.CharacterViewSet);

            UseModel(Character.Get(data.CharacterId).GetModel(data.CharacterViewSet));
            //UseSpellIcons(data.SpellIcons);

            _units[id] = this;
            gameObject.SetActive(true);
        }

        public static Unit GetUnit(int id)
        {
            return _units.GetValueOrDefault(id, null);
        }

        private void UseModel(GameObject model)
        {
            _animator = Instantiate(model, transform).GetComponent<Animator>();
        }

        public Sprite GetAbilityIcon(SpellSlot slot) => _spellIcons[(int) slot];
    }
}
