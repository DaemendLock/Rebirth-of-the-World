using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class AttributeOwnerUpdateAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IHealthOwnerRepository _healthOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IAttributesModificationCalculator _statusAttributeCalculator;

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList, IAttributesModificationCalculator statusAttributeCalculator)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
            _statusAttributeCalculator = statusAttributeCalculator;
        }

        public void Execute(IReadOnlyCollection<Updatable> targets)
        {
            System.Span<UnitId> values = stackalloc UnitId[targets.Count];
            int i = 0;

            foreach (var target in targets)
            {
                values[i++] = target.Id;
            }

            foreach (UnitId value in values)
            {
                AttributesOwner attributesOwner;

                try
                {
                    attributesOwner = _attributesRepository.Get(value);
                }
                catch
                {
                    continue;
                }

                attributesOwner.Clear();
            }

            foreach (UnitId value in values)
            {
                AttributesOwner attributesOwner;

                try
                {
                    attributesOwner = _attributesRepository.Get(value);
                }
                catch
                {
                    continue;
                }

                if (_statusOwnerRepository.TryGet(value, out StatusOwner statusOwner) == false)
                {
                    continue;
                }

                AttributesBonuses finalModification = _statusAttributeCalculator.Evaluate(statusOwner.GetAll().Select(value => value.Id));

                for (int j = 0; j < attributesOwner.BaseValues.Length; j++)
                {
                    attributesOwner.SetModification((UnitAttribute)i, finalModification[(UnitAttribute)i]);
                }

                _characterUpdateList.Update(new(value, Math.Max(1 + (finalModification.TimeScale / 100f), 0f)));
                ref HealthOwner healthOwner = ref _healthOwnerRepository.Get(value);
                healthOwner.BonusHealth = attributesOwner.GetMaxHealthBonus();
            }
        }
    }
}
