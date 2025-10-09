using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IFrameDataRepository
    {
        SkillFrameData Get(SkillId id);
    }
}
