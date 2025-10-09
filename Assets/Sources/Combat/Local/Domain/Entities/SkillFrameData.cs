using CastStateSkill;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{

    public readonly ref struct SkillFrameData
    {
        public readonly SkillId Id;

        private readonly IFrameData _frameData;

        public SkillFrameData(SkillId id, IFrameData frameData)
        {
            Id = id;
            _frameData = frameData;
        }

        public IFrameData FrameData => _frameData;
    }
}
