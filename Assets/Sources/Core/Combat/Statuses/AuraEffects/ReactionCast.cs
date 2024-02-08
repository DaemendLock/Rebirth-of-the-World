﻿using Core.Combat.Abilities;
using Core.Combat.Utils;
using Core.Combat.Utils.Serialization;
using System.IO;
using UnityEngine;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
    public class ReactionCast : DynamicEffect
    {
        private readonly int _spell;

        public ReactionCast(int spellId, UnitAction action) : base(action)
        {
            _spell = spellId;
        }

        public ReactionCast(BinaryReader source) : base((UnitAction) source.ReadInt32())
        {
            _spell = source.ReadInt32();
        }

        public override void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.OnEventCast);
            buffer.Write((int) Action);
            buffer.Write(_spell);
        }

        public override void Update(Status status, CastInputData data)
        {
            //status.Parent.CastSpell(new CastEventData(data.Caster, data.Target, Spell.Get((SpellId) _spell)));

        }
    }
}
