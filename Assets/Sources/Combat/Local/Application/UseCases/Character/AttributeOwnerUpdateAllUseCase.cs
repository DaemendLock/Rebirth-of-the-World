using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.Services.Skills;
using Combat.Local.Application.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class AttributeOwnerUpdateAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IAttributesModificationCalculator _statusAttributeCalculator;

        private readonly AttributeOwnerOperations _attributeOwnerOperations;

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList, IAttributesModificationCalculator statusAttributeCalculator)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
            _statusAttributeCalculator = statusAttributeCalculator;

            _attributeOwnerOperations = new(_attributesRepository);
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

                _attributeOwnerOperations.Clear(attributesOwner);
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

                AttributesModification finalModification = _statusAttributeCalculator.Evaluate(statusOwner.GetAll().Select(value => value.Id));
                _attributeOwnerOperations.Cache(attributesOwner, finalModification);
                _characterUpdateList.Update(new(value, Math.Max(1 + (finalModification.TimeScale / 100f), 0f)));
            }
        }
    }
}
