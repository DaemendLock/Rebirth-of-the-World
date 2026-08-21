using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public sealed class AttributeOwnerUpdateAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IStatusAttributeCalculator _statusAttributeCalculator;

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList, IStatusAttributeCalculator statusAttributeCalculator)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
            _statusAttributeCalculator = statusAttributeCalculator;
        }

        public void Execute(IReadOnlyCollection<Updatable> targets)
        {
            Span<AttributesOwner> values = _attributesRepository.GetAll();

            for (int i = 0; i < targets.Count; i++)
            {
                ref AttributesOwner target = ref values[i];
                target.Clear();
            }

            for (int i = 0; i < targets.Count; i++)
            {
                UnitId value = values[i].Id;
                StatusOwner statusOwner = _statusOwnerRepository.Get(value);

                ref AttributesOwner attributesOwner = ref values[i];
                AttributesModification finalModification = _statusAttributeCalculator.Evaluate(statusOwner.GetAll());
                attributesOwner.ApplyModifications(finalModification);
                _characterUpdateList.Update(new(value, Math.Max(1 + (finalModification.TimeScale / 100f), 0f)));
            }
        }
    }
}
