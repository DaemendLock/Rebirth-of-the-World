using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.UseCases.Scene;

using System.Collections.Generic;
using System.Linq;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly StatusOwnerProgressAllUseCases _updateStatusesUseCase;
        private readonly SkillOwnerProgressAllUseCase _skillUpdateAllUseCase;
        private readonly ComposureRecoverAllUseCase _composureRecoverAllUseCase;
        private readonly ActorActAllUseCase _actorUpdateAllUseCase;
        private readonly AttributeOwnerUpdateAllUseCase _attributeOwnerUpdateAllUseCase;
        private readonly ActorEvaluateConsciousStateAllUseCase _updateConsciousStateAllUseCase;
        private readonly HitsHandleUseCase _handleHitUseCase;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly IHitRecordQueue _hitRecordQueue;
        private readonly UnitDeleteUseCase _characterDeleteUseCase;
        private readonly IEncounterStateMachine _encounterState;
        public UpdateController(AttributeOwnerUpdateAllUseCase updateCombatUseCase,
                                StatusOwnerProgressAllUseCases updateStatusesUseCase,
                                HitsHandleUseCase handleHitUseCase,
                                ICharacterUpdateRepository characterUpdateList,
                                ActorActAllUseCase actorActAllUseCase,
                                ActorEvaluateConsciousStateAllUseCase updateConsciousStateAllUseCase,
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
            _characterUpdateList = characterUpdateList;
            _actorUpdateAllUseCase = actorActAllUseCase;
            _composureRecoverAllUseCase = composureRecoverAllUseCase;
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

            while (_characterDeleteQueue.TryDequeue(out var characterDelete))
            {
                _characterDeleteUseCase.Execute(characterDelete);
            }

            float deltaTime = UnityEngine.Time.deltaTime;
            Updatable[] updateList = _characterUpdateList.GetAll().ToArray();

            _composureRecoverAllUseCase.Execute(updateList, deltaTime);
            _skillUpdateAllUseCase.Execute(updateList, deltaTime);

            while (_hitRecordQueue.TryDequeue(out var hitRecord))
            {
                _handleHitUseCase.Execute(new(hitRecord));
            }

            _updateStatusesUseCase.Execute(updateList, deltaTime);
            _actorUpdateAllUseCase.Execute(updateList, deltaTime);
            _attributeOwnerUpdateAllUseCase.Execute(updateList);
            _updateConsciousStateAllUseCase.Execute(updateList, deltaTime);
        }

        public void UpdateCombat()
        {
            UpdateContext updateContext = new();
            List<ICombatPhase> steps = new();

            foreach (var step in steps)
            {
                step.Execute(updateContext);
            }
        }
    }

    public readonly struct UpdateContext
    {
        public readonly Updatable[] Targets;
    }

    public interface ICombatPhase
    {
        void Execute(UpdateContext updateContext);
    }

    public sealed class PerformCleanup : ICombatPhase
    {
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly UnitDeleteUseCase _characterDeleteUseCase;

        public void Execute(UpdateContext updateContext)
        {
            while (_characterDeleteQueue.TryDequeue(out var characterDelete))
            {
                _characterDeleteUseCase.Execute(characterDelete);
            }
        }
    }

    public sealed class CacheAttributes : ICombatPhase
    {
        public void Execute(UpdateContext updateContext)
        {

        }
    }
}
