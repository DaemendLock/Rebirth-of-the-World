using Core.Combat.Abilities.SpellEffects;
using System;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    public interface ActionRecord
    {
        ActionType Type { get; }
        int Target { get; }
        SpellId Source { get; }

        byte[] GetBytes();

        sealed void AddTo(IActionRecordContainer container)
        {
            container.AddAction(this);
        }
    }

    public class DummyActionRecord : ActionRecord
    {
        public ActionType Type => ActionType.Dummy;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            return new byte[0];
        }
    }

    public class ModifyResourceRecord : ActionRecord
    {
        private readonly ResourceType _resource;
        private readonly float _modification;

        public ModifyResourceRecord(EffectApplicationData data, ResourceType resource, float modififcation)
        {
            Target = data.Target.Id;
            Source = data.Source;
            _resource = resource;
            _modification = modififcation;

        }

        public ActionType Type => ActionType.ModifyResource;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            byte[] result = new byte[6];

            BitConverter.GetBytes((ushort) _resource).CopyTo(result, 0);
            BitConverter.GetBytes(_modification).CopyTo(result, sizeof(ushort));

            return result;
        }
    }

    public class AuraApplicationRecord : ActionRecord
    {
        private readonly int _casterId;
        private readonly SpellId _auraId;
        private readonly long _duration;

        public AuraApplicationRecord(EffectApplicationData data, SpellId auraId, long duration)
        {
            _casterId = data.Caster.Id;
            _auraId = auraId;
            _duration = duration;
            Target = data.Target.Id;
            Source = data.Source;
        }

        public ActionType Type => ActionType.ApplyAura;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(_casterId).CopyTo(bytes, 0);
            BitConverter.GetBytes(_auraId).CopyTo(bytes, sizeof(int));
            BitConverter.GetBytes(_duration).CopyTo(bytes, sizeof(int) << 1);

            return bytes;
        }
    }

    public class RemoveAuraActionRecord : ActionRecord
    {
        public ActionType Type => ActionType.RemoveAura;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }

    public class HealthModificationRecord : ActionRecord, IActionRecordContainer
    {
        private readonly float _newHealthValue;
        private readonly IActionRecordContainer _reactions = new ActionRecordContainer();

        public HealthModificationRecord(EffectApplicationData data, float newHealthValue, float modification)
        {
            Target = data.Target.Id;
            Source = data.Source;
            _newHealthValue = newHealthValue;
            Value = modification;
        }

        public float Value { get; }

        public ActionType Type => ActionType.HealthModification;

        public int Target { get; }

        public SpellId Source { get; }

        public void AddAction(ActionRecord action) => _reactions.AddAction(action);

        public byte[] GetBytes() => BitConverter.GetBytes(_newHealthValue);

        public void Write(BinaryWriter target) => _reactions.Write(target);
    }
}