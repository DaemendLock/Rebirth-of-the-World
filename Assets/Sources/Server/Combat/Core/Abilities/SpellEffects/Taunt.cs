﻿using Core.Combat.Abilities.ActionRecords;
using System.IO;
using Utils.ByteHelper;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Taunt : SpellEffect
    {
        public Taunt()
        {

        }

        public Taunt(ByteReader source)
        {
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
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