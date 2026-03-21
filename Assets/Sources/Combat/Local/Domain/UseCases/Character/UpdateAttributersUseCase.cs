using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using Local.Domain.Entities.Statuses.Effects;

using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public class UpdateAttributersUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICharacterUpdateList _characterUpdateList;

        public UpdateAttributersUseCase(IAttributesRepository attributesRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterUpdateList characterUpdateList)
        {
            _attributesRepository = attributesRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
            _characterUpdateList = characterUpdateList;
        }

        public void Execute(IReadOnlyCollection<Updatable> targets)
        {
            foreach (Updatable value in targets)
            {
                UpdateStatusOwner(value);
            }
        }

        private void UpdateStatusOwner(Updatable target)
        {
            AttributesOwner attributesOwner = _attributesRepository.Get(target.Id);
            StatusOwner statusOwner = _statusOwnerRepository.Get(target.Id);

            System.ReadOnlySpan<AttributeValue> baseValues = attributesOwner.GetAllBase();

            attributesOwner = new(target.Id, baseValues);
            _attributesRepository.Update(attributesOwner);
            AttributesModification finalModification = new();
            float timeScale = 100f;

            foreach (StatusId statusId in statusOwner.GetAll())
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Strategy.TryGetEffect(out ModifyAttributesEffect attributeEffect))
                {
                    AttributesModification modification = attributeEffect.ModifyAttributes();
                    finalModification += modification;
                }

                if (status.Strategy.TryGetEffect(out ModifyTimeScaleEffect timeScaleEffect))
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

            AttributesOwner newOwner = new(target.Id, baseValues, values);
            _attributesRepository.Update(newOwner);
            target = new(target.Id, timeScale / 100f);
            _characterUpdateList.Update(target);
        }
    }
}
