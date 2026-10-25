using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.UseCases.Scene;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly StatusOwnerProgressAllUseCases _updateStatusesUseCase;
        private readonly SkillOwnerProgressAllUseCase _skillUpdateAllUseCase;
        private readonly ComposureRecoverAllUseCase _composureUpdateAllUseCase;
        private readonly ActorActAllUseCase _actorUpdateAllUseCase;
        private readonly AttributeOwnerEvaluateAllUseCase _attributeOwnerUpdateAllUseCase;
        private readonly LifeStateEvaluateAllUseCase _updateConsciousStateAllUseCase;
        private readonly HitsHandleUseCase _handleHitUseCase;
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly IHitRecordQueue _hitRecordQueue;
        private readonly UnitDeleteUseCase _characterDeleteUseCase;
        private readonly IEncounterStateMachine _encounterState;
        public UpdateController(AttributeOwnerEvaluateAllUseCase updateCombatUseCase,
                                StatusOwnerProgressAllUseCases updateStatusesUseCase,
                                HitsHandleUseCase handleHitUseCase,
                                ActorActAllUseCase actorActAllUseCase,
                                LifeStateEvaluateAllUseCase updateConsciousStateAllUseCase,
                                ComposureRecoverAllUseCase composureRecoverAllUseCase,
                                SkillOwnerProgressAllUseCase skillUpdateAllUseCase,
                                ICharacterDeleteQueue characterDeleteQueue,
                                UnitDeleteUseCase characterDeleteUseCase,
                                IEncounterStateMachine encounterState,
                                IHitRecordQueue hitRecordQueue)
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
        }

        public void Tick()
        {
            if (_encounterState.IsRunning == false)
            {
                return;
            }

            Updatable[] updateList = PrepairTick();

            float deltaTime = UnityEngine.Time.deltaTime;

            _composureUpdateAllUseCase.Execute(updateList, deltaTime);
            _skillUpdateAllUseCase.Execute(updateList, deltaTime);
            _updateStatusesUseCase.Execute(updateList, deltaTime);

            while (_hitRecordQueue.TryDequeue(out var hitRecord))
            {
                _handleHitUseCase.Execute(new(hitRecord));
            }

            _actorUpdateAllUseCase.Execute(updateList, deltaTime);
            _updateConsciousStateAllUseCase.Execute(updateList);

            FinalizeTick(updateList);
        }

        private Updatable[] PrepairTick()
        {
            Updatable[] updateList = _attributeOwnerUpdateAllUseCase.Execute();
            return updateList;
        }

        private void FinalizeTick(Updatable[] updateList)
        {
            while (_characterDeleteQueue.TryDequeue(out var characterDelete))
            {
                _characterDeleteUseCase.Execute(characterDelete);
            }
        }
    }
}
