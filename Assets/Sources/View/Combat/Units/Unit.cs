using Data.Model;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataTypes;
using View.Combat.Units.Models;
using View.Combat.Units.Spells;
using CUnit = Core.Combat.Units.Unit;

namespace View.Combat.Units
{
    public class Unit : MonoBehaviour
    {
        private static Dictionary<int, Unit> _units = new();

        private CUnit _assignedUnit;
        private int _id;

        private SpellIconsSet _icons;

        private void Update()
        {
            Utils.DataTypes.Vector3 pos = _assignedUnit.Position;
            transform.position = new(pos.x, pos.y, pos.z);

            Utils.DataTypes.Vector3 rotation = new(_assignedUnit.Rotation);

            transform.forward = new UnityEngine.Vector3(rotation.x, rotation.y, rotation.z);
        }

        private void OnDestroy()
        {
            if (_assignedUnit == null)
            {
                return;
            }

            _units.Remove(_id);
        }

        public void Init(int id, UnitCreationData.ViewData data)
        {
            if (_assignedUnit != null)
            {
                return;
            }

            _assignedUnit = Core.Combat.Engine.Combat.GetUnit(id);
            _id = id;

            UseModel(ModelLibrary.Get(data.ModelId));
            UseSpellIcons(data.SpellIcons);

            _units[id] = this;
            gameObject.SetActive(true);
        }

        public static Unit GetUnit(int id)
        {
            return _units.GetValueOrDefault(id, null);
        }

        private void UseModel(Model model)
        {
            Instantiate(model.GetDefaultModel(), transform);
        }

        private void UseSpellIcons(SpellIconsSet icons)
        {
            _icons = icons;
        }

        private void UseVoiceover()
        {

        }
    }
}
