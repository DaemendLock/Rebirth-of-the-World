using CastStateSkill;

using Combat.Common.ValueObjects;

using System.Linq;

namespace Combat.Local.Domain.Entities
{
    public interface IActionStrategy
    {
        ActionState State { get; }
        float EffectiveTime { get; }

        void Start();
        void Progress(float deltaTime);
        void Interrupt();

        bool CanChainInto(SkillId skillId);
    }

    public class FrameDataActionStrategy : IActionStrategy
    {
        private readonly IFrameData _frameData;
        private readonly SkillId[] _chainableSkills;

        public FrameDataActionStrategy(IFrameData frameData)
        {
            _frameData = frameData;
            _chainableSkills = new SkillId[] { new(1001) };
            State = ActionState.Inactive;
            EffectiveTime = 0;
        }

        public ActionState State { get; set; }

        public float EffectiveTime { get; set; }

        public void Start()
        {
            if (State != ActionState.Inactive)
            {
                return;
            }

            State = ActionState.Startup;
        }

        public void Progress(float deltaTime)
        {
            EffectiveTime += deltaTime;

            if (State == ActionState.Inactive)
            {
                return;
            }

            State = (ActionState)_frameData.GetCastState(EffectiveTime);
        }

        public void Interrupt()
        {
            State = ActionState.Inactive;
        }

        public bool CanChainInto(SkillId skillId) => _chainableSkills.Contains(skillId);
    }
}
