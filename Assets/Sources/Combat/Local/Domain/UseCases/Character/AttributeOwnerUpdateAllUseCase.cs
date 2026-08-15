using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

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

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterUpdateRepository characterUpdateList, IStatusAttributeCalculator statusAttributeCalculator)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
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
                ClearTarget(value);
            }

            foreach (UnitId value in values)
            {
                UpdateTarget(value);
            }
        }

        private void ClearTarget(UnitId target)
        {
            AttributesOwner attributesOwner;

            try
            {
                attributesOwner = _attributesRepository.Get(target);
            }
            catch
            {
                return;
            }

            attributesOwner = new(attributesOwner.Id, attributesOwner.GetAllBase());
            _attributesRepository.Update(attributesOwner);
        }

        private void UpdateTarget(UnitId target)
        {
            AttributesOwner attributesOwner;

            try
            {
                attributesOwner = _attributesRepository.Get(target);
            }
            catch
            {
                return;
            }

            if (_statusOwnerRepository.TryGet(target, out StatusOwner statusOwner) == false)
            {
                return;
            }

            System.ReadOnlySpan<AttributeValue> baseValues = attributesOwner.GetAllBase();

            attributesOwner = new(target, baseValues);

            var statuses = statusOwner.GetAll();
            AttributesModification finalModification = GetModfication(statuses);

            System.Span<float> values = stackalloc float[baseValues.Length];

            for (int i = 0; i < baseValues.Length; i++)
            {
                AttributeModifier modifier = finalModification[(UnitAttribute)i];
                values[i] = (baseValues[i].BaseValue + modifier.BaseValue) * (baseValues[i].Percent + modifier.Percent) / 100f + modifier.BonusValue;
            }

            AttributesOwner newOwner = new(target, baseValues, values);
            _attributesRepository.Update(newOwner);
            _characterUpdateList.Update(new(target, finalModification.TimeScale / 100f));
        }

        private AttributesModification GetModfication(System.ReadOnlySpan<StatusId> statuses) => _statusAttributeCalculator.Evaluate(statuses);
    }
}
