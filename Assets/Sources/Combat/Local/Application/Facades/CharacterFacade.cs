using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.UseCases;

using UnityEngine;

namespace Combat.Local.Application.Facades
{
    public readonly struct CharacterFacade
    {
        private readonly ResourceGiveUseCase _giveResourceUseCase;
        private readonly ResourceSpendUseCase _spendResourceUseCase;

        private readonly ActorForceKillUseCase _killUnitUseCase;
        private readonly ActorReviveUseCase _reviveUnitUseCase;
        private readonly AddMovementEffectUseCase _addMovementEffectUseCase;
        private readonly PositionableStartScaleUseCase _startScaleUseCase;
        private readonly PositionableStopScaleUseCase _stopScaleUseCase;

        private readonly StatusOwnerApplyUseCase _applyStatusUseCase;

        public CharacterFacade(ResourceGiveUseCase giveResourceUseCase, ResourceSpendUseCase spendResourceUseCase,
                               ActorForceKillUseCase killUnitUseCase, StatusOwnerApplyUseCase applyStatusUseCase,
                               AddMovementEffectUseCase addMovementEffectUseCase, ActorReviveUseCase reviveUnitUseCase,
                               PositionableStartScaleUseCase startScaleUseCase, PositionableStopScaleUseCase stopScaleUseCase)
        {
            _giveResourceUseCase = giveResourceUseCase;
            _spendResourceUseCase = spendResourceUseCase;
            _killUnitUseCase = killUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
            _addMovementEffectUseCase = addMovementEffectUseCase;
            _reviveUnitUseCase = reviveUnitUseCase;
            _startScaleUseCase = startScaleUseCase;
            _stopScaleUseCase = stopScaleUseCase;
        }

        public void GiveResource(UnitId target, ResourceId resource, float value, AbilityKey? abilityId) =>
            _giveResourceUseCase.Execute(target, resource, value, abilityId);

        public void SpendResource(UnitId target, ResourceId resource, float value, AbilityKey? abilityId) =>
            _spendResourceUseCase.Execute(target, resource, value, abilityId);

        public void Kill(UnitId target, AbilityKey? source) => _killUnitUseCase.Execute(target, source);

        public void Revive(UnitId target, AbilityKey? source) => _reviveUnitUseCase.Execute(target, source);

        public void ApplyStatus(UnitId target, StatusType name, int stackCount, float duration, AbilityKey? source)
        {
            ApplyStatusInfo dto = new(target, name, duration, stackCount, source);
            _applyStatusUseCase.Execute(dto);
        }

        public void AddMoveInDirectionEffect(UnitId target, Vector3 direction, float speed, bool isRelative, float maxDuration = 10f) =>
            _addMovementEffectUseCase.Execute(target, direction, speed, isRelative, maxDuration);

        public ScaleEffectId StartScaleOverTime(UnitId target, float rate) => _startScaleUseCase.Execute(target, rate);

        public void StopScaleOverTime(ScaleEffectId effectId) => _stopScaleUseCase.Execute(effectId);
    }
}
