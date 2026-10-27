using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using System;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class AttributeOwnerEvaluateAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ICharacterUpdateQueue _characterUpdateList;
        private readonly IAttributesModificationCalculator _statusAttributeCalculator;
        private readonly IPositionableRepository _positionableRepository;
        private readonly ICharacterLocalTimeScaleOutput _characterLocalTimeScaleOutput;

        public AttributeOwnerEvaluateAllUseCase(IUnitStore unitStore, ICharacterUpdateQueue characterUpdateList, IAttributesModificationCalculator statusAttributeCalculator, IPositionableRepository positionableRepository, ICharacterLocalTimeScaleOutput characterLocalTimeScaleOutput)
        {
            _unitStore = unitStore;
            _characterUpdateList = characterUpdateList;
            _statusAttributeCalculator = statusAttributeCalculator;
            _positionableRepository = positionableRepository;
            _characterLocalTimeScaleOutput = characterLocalTimeScaleOutput;
        }

        public Updatable[] Execute()
        {
            Updatable[] targets = _characterUpdateList.GetAll().Select(value => new Updatable(value, 1f)).ToArray();

            for (int i = 0; i < targets.Length; i++)
            {
                if (_unitStore.TryGet(targets[i].Id, out UnitContext unit) == false)
                {
                    continue;
                }

                unit.Attributes.Clear();
            }

            for (int i = 0; i < targets.Length; i++)
            {
                ref Updatable target = ref targets[i];

                if (_unitStore.TryGet(target.Id, out UnitContext unit) == false)
                {
                    continue;
                }

                StatusBonuses finalModification = _statusAttributeCalculator.Evaluate(unit.Statuses.GetAll().Select(value => value.Id));

                for (int j = 0; j < unit.Attributes.BaseValues.Length; j++)
                {
                    unit.Attributes.SetModification((UnitAttribute)j, finalModification.Attributes[(UnitAttribute)j]);
                }

                float scale = Math.Max(1 + (finalModification.TimeScale / 100f), 0f);
                target = new(target.Id, scale);

                ApplyBonuses(unit, finalModification);

                _characterLocalTimeScaleOutput.Present(unit.Id, scale);
            }

            return targets;
        }

        private void ApplyBonuses(UnitContext unit, StatusBonuses modification)
        {
            float endurance = unit.Attributes.GetAttributeValue(UnitAttribute.Endurance);
            unit.Health.BonusHealth = unit.Attributes.GetMaxHealthBonus();
            unit.Composure.BonusComposure = unit.Attributes.GetMaxComposureBonus();
            unit.Composure.BaseDrainModifier = 1 + (endurance * 0.002f);

            AttributeModifcation movespeedBonus = modification.Movespeed;
            Positionable positionable = _positionableRepository.Get(unit.Id);
            positionable.Speed = (unit.Actor.BaseMovespeed + unit.Attributes.GetSwiftnessMovespeedBonus() + movespeedBonus.BaseValue) * (1 + movespeedBonus.Percent / 100) + movespeedBonus.BonusValue;
            _positionableRepository.Update(positionable);
        }
    }
}
