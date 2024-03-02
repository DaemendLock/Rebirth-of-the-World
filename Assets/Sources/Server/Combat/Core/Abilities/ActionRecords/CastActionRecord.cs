using Core.Combat.Abilities.SpellEffects;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    public class CastActionRecord : IActionRecordContainer, ActionRecord
    {
        private readonly int _caster;
        private readonly IActionRecordContainer _container;

        public CastActionRecord(EffectApplicationData data)
        {
            _caster = data.Caster.Id;
            Target = data.Target.Id;
            Source = data.Source;
            _container = new ActionRecordContainer();
        }

        public ActionType Type => ActionType.Cast;

        public int Target { get; }

        public SpellId Source { get; }

        public void AddAction(ActionRecord action)
        {
            _container.AddAction(action);
        }

        public byte[] GetBytes()
        {
            byte[] result;
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);
            {
                writer.Write(_caster);
                _container.Write(writer);
            }

            result = stream.ToArray();
            return result;
        }

        public void Write(BinaryWriter target)
        {
            target.Write((byte) Type);
            target.Write(Target);
            target.Write(Source);
            target.Write(_caster);
            _container.Write(target);
        }
    }
}