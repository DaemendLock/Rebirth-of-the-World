using System;
using Utils.DataTypes;

namespace Syncronization
{
    public static class GameEvents
    {
        public static event Action<int, Vector3> UnitMoved;
    }
}
