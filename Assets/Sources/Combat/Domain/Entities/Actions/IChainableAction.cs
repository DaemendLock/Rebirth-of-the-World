using Combat.Common.Primitives;

namespace Combat.Local.Application.Entities
{
    public interface IChainableAction
    {
        bool CanChainInto(SkillId skillId);
    }
}
