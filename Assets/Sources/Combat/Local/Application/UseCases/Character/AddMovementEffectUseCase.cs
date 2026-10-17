using Combat.Common.Primitives;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public sealed class AddMovementEffectUseCase
    {
        private readonly IMovementEffectOwnerRepository _repository;

        public AddMovementEffectUseCase(IMovementEffectOwnerRepository repository)
        {
            _repository = repository;
        }

        //TODO: DTO
        public void Execute(UnitId targetId, Vector3 direction, float speed, bool relative, float maxDuration)
        {
            MovementEffectOwner movementEffectOwner = _repository.Get(targetId);

            MoveInDirectionEffect effect = new(direction.normalized * speed, relative, maxDuration);
            movementEffectOwner.Effects.Add(effect);
        }
    }
}
