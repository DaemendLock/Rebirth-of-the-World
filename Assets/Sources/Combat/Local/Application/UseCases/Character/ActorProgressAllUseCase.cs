using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorProgressAllUseCase
    {
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;
        private readonly ISkillHitHandler _skillHitHandler;

        private readonly IActorActionUpdateOutput _actorUpdateOutput;

        public ActorProgressAllUseCase(IActorActionUpdateOutput actionUpdateOutput, ISkillActionStateChangeHandler skillActionStateChangeHandler, ISkillHitHandler skillHitHandler)
        {
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
            _skillHitHandler = skillHitHandler;
            _actorUpdateOutput = actionUpdateOutput;
        }

        public void Execute(System.ReadOnlySpan<UnitContext> targets, float deltaTime)
        {
            foreach (UnitContext target in targets)
            {
                try
                {
                    HandleTarget(target, deltaTime * target.Attributes.TimeScale);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        private void HandleTarget(UnitContext unit, float deltaTime)
        {
            var activity = unit.Actor.CurrentActivity;

            switch (activity.Type)
            {
                case ActivityType.Idle:
                    return;
                case ActivityType.Casting:
                    HandleCast(unit, deltaTime);
                    return;
            }
        }

        private void HandleCast(UnitContext unit, float deltaTime)
        {
            ref CastActivity cast = ref unit.Actor.CastActivity;

            if (unit.LifeState.State != ConsciousState.Alive)
            {
                ClearAction(unit, cast);
                return;
            }

            CastStateSkill.SkillCastState oldState = cast.FrameType;

            if (oldState == CastStateSkill.SkillCastState.Inactive)
            {
                FinishAction(unit, cast);
                return;
            }

            cast.Progress(deltaTime);

            if (cast.FrameType == oldState)
            {
                return;
            }

            HandleStateChanges(unit.Id, cast.Skill, (ActionState)cast.FrameType);
        }

        private void FinishAction(UnitContext unit, CastActivity cast)
        {
            unit.Actor.StopAction();
            SetInactive(unit, cast);
        }

        private void ClearAction(UnitContext unit, CastActivity cast)
        {
            unit.Actor.TryInterrupt(unit.LifeState.State == ConsciousState.Dead ? InterruptReason.Death : InterruptReason.Downed, out _);
            SetInactive(unit, cast);
        }

        private void SetInactive(UnitContext unit, CastActivity cast)
        {
            _actorUpdateOutput.Clear(unit.Id);
            HandleStateChanges(unit.Id, cast.Skill, ActionState.Inactive);
        }

        private void HandleStateChanges(UnitId actorId, SkillId skillId, ActionState state)
        {
            AbilityKey key = new(actorId, skillId);
            _skillHitHandler.Reset(key);
            _skillActionStateChangeHandler.Handle(key, state);
        }
    }
}
