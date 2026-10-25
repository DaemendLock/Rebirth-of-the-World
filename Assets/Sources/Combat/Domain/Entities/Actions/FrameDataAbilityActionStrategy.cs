using CastStateSkill;

using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using System.Linq;

namespace Combat.Local.Application.Entities
{
    public struct SkillActionHandler
    {
        private enum HoldState
        {
            Disabled,
            WaitingForRecovery,
            OnHold,
            Recovering
        }

        private readonly IFrameData _frameData;
        private bool _released;
        private HoldState _holdState;

        public SkillId SkillId { get; }

        public ActionState State { get; private set; }

        public float EffectiveTime { get; private set; }

        public void Release() => _released = true;

        public void Progress(float deltaTime)
        {
            switch (_holdState)
            {
                case HoldState.Disabled:
                case HoldState.Recovering:
                    ProgressTimeline(deltaTime);
                    return;

                case HoldState.WaitingForRecovery:
                    ProgressTimeline(deltaTime);

                    if (EffectiveTime >= _frameData.RecoveryEnterTime)
                    {
                        EffectiveTime = _frameData.RecoveryEnterTime;
                        State = ActionState.Active;
                        _holdState = HoldState.OnHold;
                    }

                    return;

                case HoldState.OnHold:
                    if (_released == false)
                    {
                        return;
                    }

                    EffectiveTime = _frameData.RecoveryEnterTime;
                    State = ActionState.Recovery;
                    _holdState = HoldState.Recovering;
                    ProgressTimeline(deltaTime);
                    return;
            }
        }

        private void ProgressTimeline(float deltaTime)
        {
            EffectiveTime += deltaTime;
            State = (ActionState)_frameData.GetCastState(EffectiveTime);
        }
    }

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
