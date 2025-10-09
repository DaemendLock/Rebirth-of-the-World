using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Data.Entities
{
    public interface ISkillData
    {
        SkillId Id { get; }
        float Cooldown { get; }
        SkillFlags Flags { get; }
        IFrameData FrameData { get; }
        string ScriptName { get; }
    }
}
