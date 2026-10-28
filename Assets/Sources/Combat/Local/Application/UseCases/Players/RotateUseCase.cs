using Combat.Common;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public class RotateUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ICharacterMotionOutput _characterMotionOutput;

        public RotateUseCase(IUnitStore unitStore, ICharacterMotionOutput characterMotionOutput)
        {
            _unitStore = unitStore;
            _characterMotionOutput = characterMotionOutput;
        }

        public void Execute(UnitId target, Vector2 angles)
        {
            UnitContext unit = _unitStore.Get(target);

            if (unit.Actor.CanMove == false || unit.LifeState.State == ConsciousState.Dead)
            {
                return;
            }

            _characterMotionOutput.Rotate(target, angles.x);
        }
    }
}
