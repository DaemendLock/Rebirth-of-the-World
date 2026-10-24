using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Common;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public class RotateUseCase
    {
        private readonly IPositionableRepository _positionableRepository;
        private readonly IUnitStore _unitStore;

        public RotateUseCase(IPositionableRepository positionableRepository, IUnitStore unitStore)
        {
            _positionableRepository = positionableRepository;
            _unitStore = unitStore;
        }

        public void Execute(UnitId target, Vector2 angles)
        {
            ref UnitContext unit = ref _unitStore.Get(target);

            if (unit.Actor.CanMove == false || unit.LifeState.State == ConsciousState.Dead)
            {
                return;
            }

            Positionable positionable = _positionableRepository.Get(target);
            positionable.Rotation *= Quaternion.Euler(0, angles.x, 0);
            _positionableRepository.Update(positionable);
        }
    }
}
