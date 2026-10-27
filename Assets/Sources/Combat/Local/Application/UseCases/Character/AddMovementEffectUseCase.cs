using Combat.Common.Primitives;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public sealed class AddMovementEffectUseCase
    {
        private readonly IPositionableRepository _repository;
        private readonly ICharacterForcedMovementOutput _output;

        public AddMovementEffectUseCase(IPositionableRepository repository, ICharacterForcedMovementOutput output)
        {
            _repository = repository;
            _output = output;
        }

        //TODO: DTO
        public void Execute(UnitId targetId, Vector3 direction, float speed, bool relative, float maxDuration)
        {
            var positionable = _repository.Get(targetId);

            Vector3 vector = direction.normalized * speed;

            MoveInDirectionEffect effect = new(new(vector.x, vector.y, vector.z), relative, maxDuration);
            positionable.ApplyForcedMovement(effect);
            var (Directional, Relative) = positionable.GetForcedMovement();
            _output.Present(targetId, Directional, Relative);
        }
    }
}
