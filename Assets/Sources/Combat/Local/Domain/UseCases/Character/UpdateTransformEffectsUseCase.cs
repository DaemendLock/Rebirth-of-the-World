using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class AddMovementEffectUseCase
    {
        private readonly MoveInDirectionEffectFactory _factory;
        private readonly IMovementEffectRepository _repository;

        public AddMovementEffectUseCase(MoveInDirectionEffectFactory factory, IMovementEffectRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        //TODO: DTO
        public void Execute(EntityId targetId, Vector3 direction, float speed, bool relative, float maxDuration)
        {
            MoveInDirectionEffect effect = _factory.Create(targetId, direction, speed, relative, maxDuration);
            _repository.Create(effect);
        }
    }
}
