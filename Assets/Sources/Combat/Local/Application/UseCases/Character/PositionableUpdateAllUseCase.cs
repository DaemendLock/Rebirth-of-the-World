using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class PositionableUpdateAllUseCase
    {
        private readonly ICharacterForcedMovementOutput _forcedMovementOutput;
        private readonly IPositionableRepository _positionableRepository;

        public PositionableUpdateAllUseCase(ICharacterForcedMovementOutput forcedMovementOutput, IPositionableRepository positionableRepository)
        {
            _forcedMovementOutput = forcedMovementOutput;
            _positionableRepository = positionableRepository;
        }

        public void Execute(Updatable[] targets, float deltaTime)
        {
            foreach (var target in targets)
            {
                Positionable positionable = _positionableRepository.Get(target.Id);
                bool isDirty = positionable.ProgressMovementEffects(deltaTime * target.TimeScale);

                if (isDirty == false)
                {
                    continue;
                }

                var (Directional, Relative) = positionable.GetForcedMovement();
                _forcedMovementOutput.Present(target.Id, Directional, Relative);
            }
        }
    }
}
