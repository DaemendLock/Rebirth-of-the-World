using Combat.Local.Data.Databases;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;

using System.Collections.Generic;
using System.Linq;

using Zenject;

namespace Testing.Local
{
    public class UpdateController : ITickable
    {
        private readonly UpdateAttributersUseCase _updateAttributesUseCase;
        private readonly UpdateStatusTimersUseCase _updateStatusTimersUseCase;
        private readonly UpdateStatusesUseCases _updateStatusesUseCase;
        private readonly UpdateActorsUseCase _updateActorsUseCase;
        private readonly HandleHitsUseCase _handleHitUseCase;
        private readonly ICharacterUpdateList _characterUpdateList;
        private readonly UpdateTransformEffectsUseCase _updateMovementEffectsUseCase;

        private readonly StatusModificationProvider _statusModificationProvider;

        public UpdateController(UpdateAttributersUseCase updateCombatUseCase, UpdateStatusTimersUseCase updateStatusTimersUseCase, StatusModificationProvider statusModificationProvider, UpdateStatusesUseCases updateStatusesUseCase, UpdateActorsUseCase updateActionUseCase, HandleHitsUseCase handleHitUseCase, ICharacterUpdateList characterUpdateList, UpdateTransformEffectsUseCase updateMovementEffectsUseCase)
        {
            _updateAttributesUseCase = updateCombatUseCase;
            _updateStatusTimersUseCase = updateStatusTimersUseCase;
            _statusModificationProvider = statusModificationProvider;
            _updateStatusesUseCase = updateStatusesUseCase;
            _updateActorsUseCase = updateActionUseCase;
            _handleHitUseCase = handleHitUseCase;
            _characterUpdateList = characterUpdateList;
            _updateMovementEffectsUseCase = updateMovementEffectsUseCase;
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            IReadOnlyCollection<Updatable> updateList = _characterUpdateList.GetAll().ToArray();

            _updateAttributesUseCase.Execute(updateList);
            _updateStatusesUseCase.Execute(deltaTime, updateList);
            _updateStatusTimersUseCase.Execute(deltaTime);
            _updateActorsUseCase.Execute(deltaTime, updateList);

            foreach (var val in updateList)
            {
                _updateMovementEffectsUseCase.Execute(val.Id, deltaTime);
            }

            _handleHitUseCase.Execute();

            _statusModificationProvider.ClearCache();
        }
    }
}
