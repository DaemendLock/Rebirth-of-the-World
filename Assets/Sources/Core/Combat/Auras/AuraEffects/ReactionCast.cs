using Core.Combat.Abilities;
using Core.Combat.Utils;
using System.IO;
using Utils.DataTypes;
using Utils.Serializer;

namespace Core.Combat.Auras.AuraEffects
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
            buffer.Write((byte) AuraEffectType.REACTION_CAST);
            buffer.Write((int) Action);
            buffer.Write(_spell);
        }

        public override void Update(Status status, CastEventData data)
        {
            status.Parent.CastAbility(new CastEventData(data.Caster, data.Target, Spell.Get((SpellId) _spell)));
            throw new System.Exception(_spell.ToString());
        }
    }
}
