﻿using Core.Combat.Utils;
using System.IO;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Teleport : SpellEffect
    {
        public Teleport(BinaryReader source)
        {
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}