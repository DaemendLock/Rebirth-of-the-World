using CastStateSkill;

using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Repositories
{
    public interface IFrameDataRepository
    {
        void Add(SkillId id, IFrameData frameData);
        IFrameData Get(SkillId skillId);
    }
}
