using Client.Combat.Core.Units.Components;
using UnityEngine;

namespace Client.Combat.Core.Units
{
    public class Unit
    {
        public int Id;

        public SpellCasting SpellCasting;

        public Health Health;

        public byte Team;

        public Vector3 Position;
        public Vector3 Velocity;
    }
}
