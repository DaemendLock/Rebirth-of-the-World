using System.Collections.Generic;
using UnityEngine;

namespace Remaster.Combat
{
    public interface Updatable
    {
        void Update();
    }

    public class Combat : MonoBehaviour
    {
        public static Combat Instance { get; private set; }

        private List<Updatable> _updates = new List<Updatable>();

        private List<Unit> _units = new List<Unit>();

        private void Awake()
        {
            Instance = this;
        }

        public void RegisterUnit(Unit unit)
        {
            _updates.Add(unit);
            _units.Add(unit);
        }

        public Unit GetTargetFor(Unit unit, Spell spell)
        {
            Unit result;

            if (spell.TargetTeam != TargetTeam.ENEMY)
            {
                result = _units.Find((target) => unit.CanHelp(target));
            }
            else
            {
                result = _units.Find((target) => unit.CanHurt(target));
            }

            return result;
        }

        private void Update()
        {
            foreach (Updatable updatable in _updates)
            {
                updatable.Update();
            }
        }
    }
}
