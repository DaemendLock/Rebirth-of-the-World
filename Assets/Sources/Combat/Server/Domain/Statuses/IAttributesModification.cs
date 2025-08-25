using Server.Combat.Domain.Attributes;

namespace Server.Combat.Domain.Statuses.StatusEffects
{
    public interface IAttributesModification<T> where T : struct, System.Enum
    {
        void Apply(IAttributeCollection<T> target);
    }
}
