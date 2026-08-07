using CastStateSkill;

using Combat.Common.ValueObjects;

using System.Linq;

namespace Combat.Local.Domain.Entities
{
    public sealed class FrameDataAbilityActionStrategy : IActionStrategy, IAbilityAction, IReleasableAction
    {
        private enum HoldState
        {
            Disabled,
            WaitingForRecovery,
            Active,
            Recovering
        }

        private readonly IFrameData _frameData;
        private readonly float _recoveryStartTime;
        private readonly SkillId[] _chainableSkills;
        private bool _releaseRequested;
        private HoldState _holdState;

        public FrameDataAbilityActionStrategy(IFrameData frameData, SkillId source, bool holdable)
        {
            _frameData = frameData ?? throw new System.ArgumentNullException(nameof(frameData));
            _recoveryStartTime = frameData.RecoveryEnterTime;
            _holdState = holdable ? HoldState.WaitingForRecovery : HoldState.Disabled;
            Source = source;
            State = ActionState.Inactive;
            EffectiveTime = 0;
        }

        public SkillId Source { get; }

        public ActionState State { get; private set; }

        public float EffectiveTime { get; private set; }

        public bool IsComplete => State == ActionState.Inactive;

        public void Start()
        {
            if (State == ActionState.Inactive)
            {
                State = ActionState.Startup;
            }
        }

        public void Progress(float deltaTime)
        {
            if (State == ActionState.Inactive)
            {
                return;
            }

            switch (_holdState)
            {
                case HoldState.Disabled:
                case HoldState.Recovering:
                    ProgressTimeline(deltaTime);
                    return;

                case HoldState.WaitingForRecovery:
                    ProgressTimeline(deltaTime);

                    if (EffectiveTime >= _recoveryStartTime)
                    {
                        EffectiveTime = _recoveryStartTime;
                        State = ActionState.Active;
                        _holdState = HoldState.Active;
                    }

                    return;

                case HoldState.Active:
                    if (_releaseRequested == false)
                    {
                        return;
                    }

                    EffectiveTime = _recoveryStartTime;
                    State = ActionState.Recovery;
                    _holdState = HoldState.Recovering;
                    ProgressTimeline(deltaTime);
                    return;
            }
        }

        public void Interrupt(InterruptReason reason) => State = ActionState.Inactive;

        public void Release() => _releaseRequested = true;

        public bool CanChainInto(SkillId skillId) => _chainableSkills.Contains(skillId);

        private void ProgressTimeline(float deltaTime)
        {
            EffectiveTime += deltaTime;
            State = (ActionState)_frameData.GetCastState(EffectiveTime);
        }
    }
}
