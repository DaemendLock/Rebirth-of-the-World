using CastStateSkill;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public interface IActionStrategy
    {
        ActionState State { get; }
        float EffectiveTime { get; }

        void Start();
        void Progress(float deltaTime);
    }

    public class CastActionStrategy : IActionStrategy
    {
        private readonly IFrameData _frameData;

        public CastActionStrategy(IFrameData frameData)
        {
            _frameData = frameData;
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
    }
}
