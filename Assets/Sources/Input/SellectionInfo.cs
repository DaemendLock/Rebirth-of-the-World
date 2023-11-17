using System;
using System.Collections.Generic;

namespace Input
{
    public static class SellectionInfo
    {
        public static event Action<int> SelectionChanged;
        public static event Action TargetChanged;

        private static List<ControllUnit> _units = new();

        private static int _targetIndex = -1;
        private static int _controlIndex = -1;

        public static int ContolledUnitId { get; private set; } = -1;
        public static int TargetedUnitId { get; private set; } = -1;

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
            TargetedUnitId = unitId;
        }

        public static void TargetNext()
        {
            if (_units.Count == 0)
            {
                return;
            }

            NextIndexFromList(ref _targetIndex, _units);

            TargetedUnitId = _units[_targetIndex].Id;
            TargetChanged?.Invoke();
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

            ContolledUnitId = _units[_controlIndex].Id;
            SelectionChanged?.Invoke(ContolledUnitId);
        }

        public static void RegisterControllUnit(int id, byte controlGroup)
        {
            _units.Add(new(id, controlGroup));

            if(controlGroup == ControlGroup && _controlIndex == -1)
            {
                _controlIndex = _units.Count - 1;
                SelectionChanged?.Invoke(_controlIndex);
            }

            if (_targetIndex == -1)
            {
                _targetIndex = 0;
                TargetChanged?.Invoke();
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
