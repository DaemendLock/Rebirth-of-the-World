using Combat.Domain.Entities;
using Combat.Local.Application.Queries;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.UseCases.Character;
using Combat.Local.Application.UseCases.Scene;
using Combat.Local.Application.ValueObjects;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly StatusOwnerProgressAllUseCases _updateStatusesUseCase;
        private readonly SkillOwnerProgressAllUseCase _skillUpdateAllUseCase;
        private readonly ComposureRecoverAllUseCase _composureUpdateAllUseCase;
        private readonly ActorProgressAllUseCase _actorUpdateAllUseCase;
        private readonly TickEvaluateAllUseCase _attributeOwnerUpdateAllUseCase;
        private readonly LifeStateEvaluateAllUseCase _updateConsciousStateAllUseCase;
        private readonly HitsHandleUseCase _handleHitUseCase;
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly IHitRecordQueue _hitRecordQueue;
        private readonly UnitDeleteUseCase _characterDeleteUseCase;
        private readonly IEncounterStateMachine _encounterState;
        private readonly PositionableUpdateAllUseCase _positionableUpdateAllUseCase;
        private readonly ActorCastSkillFromSlotUseCase _actorCastSkillFromSlotUse;
        private readonly ActorReleaseSkillFromSlotUseCase _actorReleaseSkillFromSlotUseCase;

        private readonly IInputQueue _inputQueue;

        public UpdateController(TickEvaluateAllUseCase updateCombatUseCase,
                                StatusOwnerProgressAllUseCases updateStatusesUseCase,
                                HitsHandleUseCase handleHitUseCase,
                                ActorProgressAllUseCase actorActAllUseCase,
                                LifeStateEvaluateAllUseCase updateConsciousStateAllUseCase,
                                ComposureRecoverAllUseCase composureRecoverAllUseCase,
                                SkillOwnerProgressAllUseCase skillUpdateAllUseCase,
                                ICharacterDeleteQueue characterDeleteQueue,
                                UnitDeleteUseCase characterDeleteUseCase,
                                IEncounterStateMachine encounterState,
                                IHitRecordQueue hitRecordQueue,
                                PositionableUpdateAllUseCase positionableUpdateAllUseCase,
                                ActorCastSkillFromSlotUseCase actorCastUseCase,
                                IInputQueue inputQueue,
                                ActorReleaseSkillFromSlotUseCase actorReleaseSkillFromSlotUseCase)
        {
            _attributeOwnerUpdateAllUseCase = updateCombatUseCase;
            _updateStatusesUseCase = updateStatusesUseCase;
            _handleHitUseCase = handleHitUseCase;
            _actorUpdateAllUseCase = actorActAllUseCase;
            _composureUpdateAllUseCase = composureRecoverAllUseCase;
            _skillUpdateAllUseCase = skillUpdateAllUseCase;
            _characterDeleteQueue = characterDeleteQueue;
            _characterDeleteUseCase = characterDeleteUseCase;
            _encounterState = encounterState;
            _hitRecordQueue = hitRecordQueue;
            _updateConsciousStateAllUseCase = updateConsciousStateAllUseCase;
            _positionableUpdateAllUseCase = positionableUpdateAllUseCase;
            _actorCastSkillFromSlotUse = actorCastUseCase;
            _inputQueue = inputQueue;
            _actorReleaseSkillFromSlotUseCase = actorReleaseSkillFromSlotUseCase;
        }

        public void Tick()
        {
            if (_encounterState.IsRunning == false)
            {
                return;
            }

            using TickContext context = PrepairTick();
            HandleInputs();
            ProgressState(context);
            FinalizeTick(context);
        }

        private TickContext PrepairTick() => _attributeOwnerUpdateAllUseCase.Execute();

        private void HandleInputs()
        {
            while (_hitRecordQueue.TryDequeue(out var hitRecord))
            {
                _handleHitUseCase.Execute(new(hitRecord));
            }

            while (_inputQueue.TryDequeue(out var command))
            {
                switch (command)
                {
                    case CastSkillCommand cast:
                        _actorCastSkillFromSlotUse.Execute(cast.Target, cast.SkillSlot);
                        continue;
                    case ReleaseSkillCommand release:
                        _actorReleaseSkillFromSlotUseCase.Execute(release.Target, release.SkillSlot);
                        continue;
                }
            }
        }

        private void ProgressState(TickContext context)
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            _composureUpdateAllUseCase.Execute(context, deltaTime);
            _skillUpdateAllUseCase.Execute(context.Updatables, deltaTime);
            _updateStatusesUseCase.Execute(context.Updatables, deltaTime);
            _actorUpdateAllUseCase.Execute(context.Updatables, deltaTime);
            _positionableUpdateAllUseCase.Execute(context.Updatables, deltaTime);
        }

        private void FinalizeTick(TickContext context)
        {
            _updateConsciousStateAllUseCase.Execute(context.Updatables);

            while (_characterDeleteQueue.TryDequeue(out var characterDelete))
            {
                _characterDeleteUseCase.Execute(characterDelete);
            }
        }
    }
}
