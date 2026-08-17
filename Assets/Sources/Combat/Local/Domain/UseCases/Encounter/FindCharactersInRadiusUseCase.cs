using Combat.Common.Primitives;
using Combat.Local.Domain.Queries;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct FindCharactersInRadiusUseCase
    {
        private readonly IUnitSpatialQuery _positionableRepository;

        public FindCharactersInRadiusUseCase(IUnitSpatialQuery positionableRepository)
        {
            _positionableRepository = positionableRepository;
        }

        public IReadOnlyCollection<UnitId> Execute(Vector3 position, float radius)
        {
            return _positionableRepository.FindInRadius(position, radius);
        }
    }
}
