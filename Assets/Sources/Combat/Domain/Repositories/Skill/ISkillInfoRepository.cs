using Combat.Common.Primitives;
using Combat.Domain.ValueObjects;

namespace Combat.Domain.Repositories
{
    public interface ISkillInfoRepository
    {
        SkillInfo Get(SkillId skill);
    }
}
