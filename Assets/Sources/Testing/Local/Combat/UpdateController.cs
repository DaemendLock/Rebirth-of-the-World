using Combat.Local.Domain.Entities.Units;
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
        private readonly CharacterDeleteUseCase _characterDeleteUseCase;
        public UpdateController(AttributeOwnerUpdateAllUseCase updateCombatUseCase,
                                StatusOwnerProgressAllUseCases updateStatusesUseCase,
                                HitsHandleUseCase handleHitUseCase,
                                ICharacterUpdateRepository characterUpdateList,
                                ActorActAllUseCase actorActAllUseCase,
                                AbilityProgressAllUseCase skillUpdateAllUseCase,
                                ICharacterDeleteQueue characterDeleteQueue,
                                CharacterDeleteUseCase characterDeleteUseCase)
        {
            _attributeOwnerUpdateAllUseCase = updateCombatUseCase;
            _updateStatusesUseCase = updateStatusesUseCase;
            _handleHitUseCase = handleHitUseCase;
            _characterUpdateList = characterUpdateList;
            _actorUpdateAllUseCase = actorActAllUseCase;
            _skillUpdateAllUseCase = skillUpdateAllUseCase;
            _characterDeleteQueue = characterDeleteQueue;
            _characterDeleteUseCase = characterDeleteUseCase;
        }

        public void Tick()
        {
            while (_characterDeleteQueue.TryDequeue(out var characterDelete))
            {
                _characterDeleteUseCase.Execute(characterDelete);
            }

            float deltaTime = UnityEngine.Time.deltaTime;

            Updatable[] updateList = _characterUpdateList.GetAll().ToArray();
            _handleHitUseCase.Execute(updateList);

            _skillUpdateAllUseCase.Execute(updateList, deltaTime);
            _updateStatusesUseCase.Execute(updateList, deltaTime);
            _actorUpdateAllUseCase.Execute(updateList, deltaTime);

            _attributeOwnerUpdateAllUseCase.Execute(updateList);
        }

        public void UpdateCombat()
        {
            List<ICombatPhase> steps = new();

            foreach (var step in steps)
            {
                step.Execute();
            }
        }
    }

    public interface ICombatPhase
    {
        void Execute();
    }

    public sealed class PerformCleanup : ICombatPhase
    {
        public void Execute()
        {

        }
    }

    public sealed class CacheAttributes : ICombatPhase
    {
        public void Execute()
        {

        }
    }
}
