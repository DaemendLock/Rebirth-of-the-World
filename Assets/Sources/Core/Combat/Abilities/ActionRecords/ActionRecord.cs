using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    public enum ActionType
    {
        Dummy,
        SchoolDamage,
        Heal,
        ApplyAura,
        RemoveAura,
        Kill,
        Resurrect,
        Precast,
        StopCast,
        ModifyResource,
    }

    public interface ActionRecord
    {
        ActionType Type { get; }
        int Target { get; }
        SpellId Source { get; }

        byte[] GetBytes();
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
        private ResourceType _resource;
        private float _modification;

        public ModifyResourceRecord(int target, ResourceType resource, float modififcation)
        {
            Target = target;
            _resource = resource;
            _modification = modififcation;
        }

        public ActionType Type => ActionType.ModifyResource;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
    public class DamageActionRecord : ActionRecord
    {
        public DamageActionRecord(int target, SpellId source)
        {
            Target = target;
            Source = source;
        }

        public ActionType Type => ActionType.SchoolDamage;

        public int Target { get; }

        public SpellId Source { get; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}