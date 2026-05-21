using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Character;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Domain.UseCases.Skills;

using System.Linq;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly AttributeOwnerUpdateAllUseCase _attributeOwnerUpdateAllUseCase;
        private readonly StatusTimerUpdateAllUseCase _updateStatusTimersUseCase;
        private readonly StatusUpdateAllUseCases _updateStatusesUseCase;
        private readonly AbilityUpdateAllUseCase _skillUpdateAllUseCase;
        private readonly ActorActAllUseCase _actorUpdateAllUseCase;
        private readonly HitsHandleUseCase _handleHitUseCase;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        public UpdateController(AttributeOwnerUpdateAllUseCase updateCombatUseCase, StatusTimerUpdateAllUseCase updateStatusTimersUseCase, StatusUpdateAllUseCases updateStatusesUseCase, HitsHandleUseCase handleHitUseCase, ICharacterUpdateRepository characterUpdateList, ActorActAllUseCase actorActAllUseCase, AbilityUpdateAllUseCase skillUpdateAllUseCase)
        {
            _attributeOwnerUpdateAllUseCase = updateCombatUseCase;
            _updateStatusTimersUseCase = updateStatusTimersUseCase;
            _updateStatusesUseCase = updateStatusesUseCase;
            _handleHitUseCase = handleHitUseCase;
            _characterUpdateList = characterUpdateList;
            _actorUpdateAllUseCase = actorActAllUseCase;
            _skillUpdateAllUseCase = skillUpdateAllUseCase;
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            _attributeOwnerUpdateAllUseCase.Execute();

            Updatable[] updateList = _characterUpdateList.GetAll().ToArray();
 
            _skillUpdateAllUseCase.Execute(updateList, deltaTime);
            _updateStatusesUseCase.Execute(updateList, deltaTime);

            _updateStatusTimersUseCase.Execute(deltaTime);
            _handleHitUseCase.Execute(updateList);
            _actorUpdateAllUseCase.Execute(updateList, deltaTime);
        }
    }
}
