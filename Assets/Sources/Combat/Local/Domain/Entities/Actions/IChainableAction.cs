using Combat.Common.Primitives;

namespace Combat.Local.Domain.Entities
{
    public interface IChainableAction
    {
        bool CanChainInto(SkillId skillId);
    }
}
