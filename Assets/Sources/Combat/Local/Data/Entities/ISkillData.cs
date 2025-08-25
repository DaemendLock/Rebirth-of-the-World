using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Flags;

namespace Combat.Local.Data.Entities
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
