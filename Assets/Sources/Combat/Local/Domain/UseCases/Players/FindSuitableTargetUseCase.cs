using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
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
            var skill = _skillRepository.GetPropertyContainer(id);

            if (skill.TryGet(out LockTargetSkillEffect effect) == false)
            {
                return default;
            }

            var targets = FindTargets(effect);

            foreach (UnitId target in targets)
            {
                if (effect.CanTarget(target) == false)
                {
                    continue;
                }

                return target;
            }

            return default;
        }

        private IReadOnlyCollection<UnitId> FindTargets(LockTargetSkillEffect effect)
        {
            return _characterRepository.FindInRadius(UnityEngine.Vector3.zero, 15f);

            return Array.Empty<UnitId>();
        }
    }
}
