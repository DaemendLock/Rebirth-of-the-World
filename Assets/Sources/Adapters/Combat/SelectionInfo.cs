using System;
using System.Collections.Generic;

namespace Adapters.Combat
{
    public static class SelectionInfo
    {
        public static event Action SelectionChanged;

        private static List<ControllUnit> _units = new();

        private static int _targetIndex = -1;
        private static int _controlIndex = -1;

        public static int SelectionId { get; private set; } = -1;
        public static int TargetId { get; private set; } = -1;

        public static int ControlGroup { get; private set; } = 0;

        private struct ControllUnit
        {
            public readonly int Id;
            public byte ControllGroup;

            public ControllUnit(int id, byte controlGroup) : this()
            {
                Id = id;
                ControllGroup = controlGroup;
            }
        }

        public static void Target(int unitId)
        {
            TargetId = unitId;
        }

        public static void TargetNext()
        {
            if (_units.Count == 0)
            {
                return;
            }

            NextIndexFromList(ref _targetIndex, _units);

            TargetId = _units[_targetIndex].Id;
        }

        public static void TargetPrev()
        {

        }

        public static void ControlNext()
        {
            if (_units.Count == 0)
            {
                return;
            }

            do
            {
                NextIndexFromList(ref _controlIndex, _units);
            } while (ControlGroup != _units[_controlIndex].ControllGroup);

            SelectionId = _units[_controlIndex].Id;
        }

        public static void RegisterControllUnit(int id, byte controlGroup)
        {
            _units.Add(new(id, controlGroup));

            if (controlGroup == ControlGroup && _controlIndex == -1)
            {
                _controlIndex = _units.Count - 1;
            }

            if (_targetIndex == -1)
            {
                _targetIndex = 0;
            }
        }

        public static void Clear()
        {
            _units.Clear();
        }

        private static void NextIndexFromList<T>(ref int index, List<T> list)
        {
            index++;

            if (_units.Count == index)
            {
                index = 0;
            }
        }
    }
}
