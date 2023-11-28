using System.Collections.Generic;

namespace View.Combat.Units
{
    internal static class EntityPool
    {
        private static readonly List<object> _units = new();

        public static void Clear()
        {
            _units.Clear();
        }

        //TODO: specify
        public static int RegisterUnit(object unit)
        {
            _units.Add(unit);
            return _units.Count - 1;
        }

        public static object GetUnitById(int id)
        {
            return _units[id];
        }
    }
}