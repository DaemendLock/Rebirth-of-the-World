using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Character;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Domain.UseCases.Skills;

using System.Collections.Generic;
using System.Linq;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly AttributeOwnerUpdateAllUseCase _attributeOwnerUpdateAllUseCase;
        private readonly StatusOwnerProgressAllUseCases _updateStatusesUseCase;
        private readonly AbilityProgressAllUseCase _skillUpdateAllUseCase;
        private readonly ActorActAllUseCase _actorUpdateAllUseCase;
        private readonly HitsHandleUseCase _handleHitUseCase;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly IHitRecordQueue _hitRecordQueue;
        private readonly CharacterDeleteUseCase _characterDeleteUseCase;
        private readonly IEncounterStateMachine _encounterState;
        public UpdateController(AttributeOwnerUpdateAllUseCase updateCombatUseCase,
                                StatusOwnerProgressAllUseCases updateStatusesUseCase,
                                HitsHandleUseCase handleHitUseCase,
                                ICharacterUpdateRepository characterUpdateList,
                                ActorActAllUseCase actorActAllUseCase,
                                AbilityProgressAllUseCase skillUpdateAllUseCase,
                                ICharacterDeleteQueue characterDeleteQueue,
                                CharacterDeleteUseCase characterDeleteUseCase,
                                IEncounterStateMachine encounterState,
                                IHitRecordQueue hitRecordQueue)
        {
            _attributeOwnerUpdateAllUseCase = updateCombatUseCase;
            _updateStatusesUseCase = updateStatusesUseCase;
            _handleHitUseCase = handleHitUseCase;
            _characterUpdateList = characterUpdateList;
            _actorUpdateAllUseCase = actorActAllUseCase;
            _skillUpdateAllUseCase = skillUpdateAllUseCase;
            _characterDeleteQueue = characterDeleteQueue;
            _characterDeleteUseCase = characterDeleteUseCase;
            _encounterState = encounterState;
            _hitRecordQueue = hitRecordQueue;
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

            while (_hitRecordQueue.TryDequeue(out var hitRecord))
            {
                _handleHitUseCase.Execute(hitRecord);
            }

            _skillUpdateAllUseCase.Execute(updateList, deltaTime);
            _updateStatusesUseCase.Execute(updateList, deltaTime);
            _actorUpdateAllUseCase.Execute(updateList, deltaTime);

            _attributeOwnerUpdateAllUseCase.Execute(updateList);
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
        private readonly CharacterDeleteUseCase _characterDeleteUseCase;

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
