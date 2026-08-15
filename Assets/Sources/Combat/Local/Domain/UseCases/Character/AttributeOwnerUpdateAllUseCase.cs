using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services.Skills;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public sealed class AttributeOwnerUpdateAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IStatusAttributeCalculator _statusAttributeCalculator;

        private readonly AttributeOwnerOperations _attributeOwnerOperations;

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterUpdateRepository characterUpdateList, IStatusAttributeCalculator statusAttributeCalculator)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
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

                AttributesModification finalModification = _statusAttributeCalculator.Evaluate(statusOwner.GetAll());
                _attributeOwnerOperations.Cache(attributesOwner, finalModification);
                _characterUpdateList.Update(new(value, Math.Max(1 + (finalModification.TimeScale / 100f), 0f)));
            }
        }
    }
}
