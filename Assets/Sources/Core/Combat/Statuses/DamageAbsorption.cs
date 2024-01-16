﻿using Core.Combat.Abilities;

namespace Core.Combat.Statuses
{
    public struct DamageAbsorption
    {
        public DamageAbsorption(float capacity, SchoolType school)
        {
            Capacity = capacity;
            Type = school;
        }

        public float Capacity { get; set; }
        public SchoolType Type { get; private set; }
    }
}
