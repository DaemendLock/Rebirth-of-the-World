using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public class AttributeOwnerUpdateAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        public AttributeOwnerUpdateAllUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterUpdateRepository characterUpdateList)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
            _characterUpdateList = characterUpdateList;
        }

        public void Execute()
        {
            System.Collections.Generic.IReadOnlyCollection<Updatable> targets = _characterUpdateList.GetAll();
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
            AttributesOwner attributesOwner = _attributesRepository.Get(target);
            attributesOwner = new(attributesOwner.Id, attributesOwner.GetAllBase());
            _attributesRepository.Update(attributesOwner);
        }

        private void UpdateTarget(UnitId target)
        {
            AttributesOwner attributesOwner = _attributesRepository.Get(target);
            StatusOwner statusOwner = _statusOwnerRepository.Get(target);

            System.ReadOnlySpan<AttributeValue> baseValues = attributesOwner.GetAllBase();

            attributesOwner = new(target, baseValues);
            AttributesModification finalModification = new();
            float timeScale = 100f;

            foreach (StatusId statusId in statusOwner.GetAll())
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IModifyAttributesStrategy attributeEffect))
                {
                    AttributesModification modification = attributeEffect.ModifyAttributes();
                    finalModification += modification;
                }

                if (status.Properties.TryGetProperty(out IModifyTimeScaleStrategy timeScaleEffect))
                {
                    timeScale += timeScaleEffect.GetModification();
                }
            }

            System.Span<float> values = stackalloc float[baseValues.Length];

            for (int i = 0; i < baseValues.Length; i++)
            {
                AttributeModifier modifier = finalModification[(Attribute)i];
                values[i] = (baseValues[i].BaseValue + modifier.BaseValue) * (baseValues[i].Percent + modifier.Percent) / 100f + modifier.BonusValue;
            }

            AttributesOwner newOwner = new(target, baseValues, values);
            _attributesRepository.Update(newOwner);
            _characterUpdateList.Update(new(target, timeScale / 100f));
        }
    }
}
