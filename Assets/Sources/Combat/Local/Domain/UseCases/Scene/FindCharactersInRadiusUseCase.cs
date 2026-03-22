using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct FindCharactersInRadiusUseCase
    {
        private readonly IPositionableRepository _positionableRepository;

        public FindCharactersInRadiusUseCase(IPositionableRepository positionableRepository)
        {
            _positionableRepository = positionableRepository;
        }

        public ICollection<EntityId> Execute(Vector3 position, float radius)
        {
            return _positionableRepository.FindInRadius(position, radius);
        }
    }
}
