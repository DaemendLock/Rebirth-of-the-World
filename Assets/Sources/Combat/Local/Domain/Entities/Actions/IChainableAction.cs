using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public interface IChainableAction
    {
        bool CanChainInto(SkillId skillId);
    }
}
