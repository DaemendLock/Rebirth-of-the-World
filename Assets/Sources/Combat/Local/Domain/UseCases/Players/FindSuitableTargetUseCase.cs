using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases.Players
{
    public readonly struct FindSuitableTargetUseCase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPositionableRepository _characterRepository;
        private readonly IAbilityRepository _skillRepository;

        public UnitId? Execute(AbilityKey id)
        {
            //var skill = _skillRepository.GetPropertyContainer(id);

            //if (skill.TryGet(out ILockTargetBehaviour effect) == false)
            //{
            //    return default;
            //}

            //var targets = FindTargets(effect);

            //foreach (UnitId target in targets)
            //{
            //    if (effect.Handle(target) == false)
            //    {
            //        continue;
            //    }

            //    return target;
            //}

            return default;
        }

        private IReadOnlyCollection<UnitId> FindTargets(ILockTargetBehaviour effect)
        {
            return _characterRepository.FindInRadius(UnityEngine.Vector3.zero, 15f);

            return Array.Empty<UnitId>();
        }
    }
}
