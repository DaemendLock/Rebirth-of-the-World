using Remaster.Sync;
using System.Collections.Generic;

namespace Remaster.Engine
{
    public interface Updatable
    {
        void Update();
    }

    public static class Combat
    {
        private static List<Updatable> _updates = new List<Updatable>();
        private static Queue<Updatable> _registerQueue = new Queue<Updatable>();
        private static List<Unit> _units = new List<Unit>();

        private static bool _running = false;

        public static void Start()
        {
            if (_running)
            {
                return;
            }

            _running = true;

            while (_running)
            {
                Update();
            }
        }

        public static void Stop()
        {
            if (_running == false)
            {
                return;
            }

            _running = false;
        }

        public static void RegisterUnit(Unit unit)
        {
            _registerQueue.Enqueue(unit);
            _units.Add(unit);
        }

        public static Unit GetTargetFor(Unit unit, Spell spell)
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

        public static void Reset()
        {
            _updates.Clear();
            _units.Clear();

            CombatTime.Reset();
        }

        private static void Update()
        {
            UpdateInput();
            UpdateUpdateble();
            RegisterUpdateable();
        }

        private static void UpdateInput()
        {
            ModelSync.Synchronize();
        }

        private static void UpdateUpdateble()
        {
            foreach (Updatable updatable in _updates)
            {
                updatable.Update();
            }
        }

        private static void RegisterUpdateable()
        {
            if (_registerQueue.Count == 0)
            {
                return;
            }

            lock (_registerQueue)
            {
                _updates.Capacity = _updates.Count + _registerQueue.Count;

                for (int i = _updates.Count; i < _updates.Capacity; i++)
                {
                    _updates.Add(_registerQueue.Dequeue());
                }
            }
        }
    }
}
