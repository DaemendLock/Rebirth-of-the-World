﻿using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units.Components;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class CreateProjectile : SpellEffect
    {
        private readonly SpellId _spell;
        private readonly PositionComponent _position;

        private readonly float _speed;

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