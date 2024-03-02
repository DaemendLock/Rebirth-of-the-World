using Client.Combat.View.Units;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Adapters.Combat.ActionHandlers
{
    internal interface ActionHandler
    {
        public void Handle();
    }

    public abstract class AbstractActionHandler : ActionHandler
    {
        public AbstractActionHandler(ByteReader source)
        {
            Target = source.ReadInt();
            Source = (SpellId) source.ReadInt();
        }

        public abstract ActionType Type { get; }
        public int Target { get; }
        public SpellId Source { get; }

        public abstract void Handle();
    }

    internal class DummyActionHandler : ActionHandler
    {
        public DummyActionHandler(ByteReader source)
        {
        }

        public void Handle()
        {
        }
    }

    internal class CastActionHandler : AbstractActionHandler
    {
        public CastActionHandler(ByteReader source) : base(source)
        {
            CasterId = source.ReadInt();
        }

        public int CasterId { get; }

        public override ActionType Type => ActionType.Cast;

        public override void Handle()
        {
            UnitView unit = Unit.GetUnit(CasterId);
            unit.PerformCast();
        }
    }

    internal class HealthModificationActionHandler : AbstractActionHandler
    {
        private float _newHealth;

        public HealthModificationActionHandler(ByteReader source) : base(source)
        {
            _newHealth= source.ReadFloat();
        }

        public override ActionType Type => ActionType.HealthModification;

        public override void Handle()
        {
            UnitView unit = Unit.GetUnit(Target);
            unit.SetHealth(_newHealth);
        }
    }

    internal class ResourceModificationActionHandler : AbstractActionHandler
    {
        ResourceType _type;
        float _value;

        public ResourceModificationActionHandler(ByteReader source) : base(source)
        {
            _type = (ResourceType) source.ReadUShort();
            _value = source.ReadFloat();
        }

        public override ActionType Type => ActionType.ModifyResource;

        public override void Handle()
        {
            UnityEngine.Debug.Log($"Restored {_value} {_type} for Unit{Target}");
        }
    }
}
