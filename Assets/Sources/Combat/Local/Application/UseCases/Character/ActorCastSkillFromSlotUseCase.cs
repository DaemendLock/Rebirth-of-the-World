using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;

using System.Linq;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class ActorCastSkillFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ISkillInfoRepository _skillInfoRepository;
        private readonly ICastActionFactory _castActionFactory;
        private readonly IActorActionUpdateOutput _actionUpdateOutput;
        private readonly ISkillHitHandler _skillHitHandler;
        private readonly ISkillExecutionPort _skillExecutionPort;
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;

        public ActorCastSkillFromSlotUseCase(IUnitStore unitStore, ISkillInfoRepository abilityFactory, ICastActionFactory actionFactory, IActorActionUpdateOutput actorUpdateOutput, ISkillHitHandler skillHitHandler, ISkillExecutionPort skillExecutionPort, ISkillActionStateChangeHandler skillActionStateChangeHandler)
        {
            _unitStore = unitStore;
            _skillInfoRepository = abilityFactory;
            _castActionFactory = actionFactory;
            _actionUpdateOutput = actorUpdateOutput;
            _skillHitHandler = skillHitHandler;
            _skillExecutionPort = skillExecutionPort;
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
        }

        public void Execute(UnitId target, int skillSlot)
        {
            if (_unitStore.TryGet(target, out UnitContext unit) == false)
            {
                return;
            }

            var skill = unit.Skills.GetSkill(skillSlot);

            if (skill.HasValue == false)
            {
                return;
            }

            Cast(unit, skill.Value);
        }

        private void Cast(UnitContext unit, SkillId skillId)
        {
            if (unit.Skills.GetCooldown(skillId) > 0)
            {
                return;
            }

            UnitId casterId = unit.Id;
            Actor actor = unit.Actor;
            SkillInfo skillInfo = _skillInfoRepository.Get(skillId);
            CastInfo castInfo = new(casterId, skillId, skillInfo.Flags, skillInfo.Actions?.FirstOrDefault());

            if (castInfo.RequestedAction.HasValue)
            {
                CastActivity oldCast = actor.CastActivity;
                var frameType = (ActionState)oldCast.FrameType;

                if (frameType != ActionState.Recovery && frameType != ActionState.Inactive)
                {
                    return;
                }

                if (oldCast.CanChainInto(skillId) == false)
                {
                    return;
                }

                if (TryEndOldAction(actor) == false)
                {
                    return;
                }
            }

            CastFailReason reason = CanCast(castInfo, unit);

            if (reason != CastFailReason.Success)
            {
                UnityEngine.Debug.Log($"Cast failed. Reason: {reason}");
                return;
            }

            try
            {
                ActionFlags flags = _skillExecutionPort.BeginCast(castInfo);

                if (castInfo.RequestedAction.HasValue)
                {
                    CastActivity cast = _castActionFactory.Create(castInfo.RequestedAction.Value, castInfo.SkillId, castInfo.Flags, flags);
                    unit.Actor.StartCast(cast);
                    _actionUpdateOutput.Present(castInfo.Caster, castInfo.RequestedAction.Value, flags, unit.Actor.CurrentActivity.ActiveTime);
                    _skillActionStateChangeHandler.Handle(new(unit.Id, skillId), ActionState.Startup);
                }
            }
            catch
            {
                return;
            }

            if (skillInfo.BaseCooldown > 0)
            {
                unit.Skills.SetCooldown(skillId, skillInfo.BaseCooldown);
            }
        }

        private bool TryEndOldAction(Actor actor)
        {
            if (actor.TryInterrupt(InterruptReason.Chained, out var oldAction) == false)
            {
                return false;
            }

            if (oldAction.Type == ActivityType.Casting)
            {
                CastActivity cast = actor.CastActivity;
                _actionUpdateOutput.Clear(actor.Id);
                AbilityKey oldKey = new(actor.Id, cast.Skill);
                _skillHitHandler.Reset(oldKey);
                _skillActionStateChangeHandler.Handle(oldKey, ActionState.Inactive);
            }

            return true;
        }

        private CastFailReason CanCast(CastInfo abilityKey, UnitContext unit)
        {
            if (unit.LifeState.State != ConsciousState.Alive)
            {
                return CastFailReason.Dead;
            }

            Actor actor = unit.Actor;

            ActivityState activity = actor.CurrentActivity;

            if (activity.Type == ActivityType.Idle || abilityKey.Flags.HasFlag(SkillFlags.Instant))
            {
                return _skillExecutionPort.CanCast(abilityKey);
            }

            if (activity.Type != ActivityType.Idle)
            {
                return CastFailReason.CastInProgress;
            }

            return _skillExecutionPort.CanCast(abilityKey);
        }
    }
}
