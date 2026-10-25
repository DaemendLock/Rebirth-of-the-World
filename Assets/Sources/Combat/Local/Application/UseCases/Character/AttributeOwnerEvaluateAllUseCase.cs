using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
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

        public AttributeOwnerEvaluateAllUseCase(IUnitStore unitStore, ICharacterUpdateQueue characterUpdateList, IAttributesModificationCalculator statusAttributeCalculator, IPositionableRepository positionableRepository)
        {
            _unitStore = unitStore;
            _characterUpdateList = characterUpdateList;
            _statusAttributeCalculator = statusAttributeCalculator;
            _positionableRepository = positionableRepository;
        }

        public Updatable[] Execute()
        {
            Updatable[] targets = _characterUpdateList.GetAll().ToArray();

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
                _characterUpdateList.Update(new(target.Id, scale));
                target = new(target.Id, scale);

                unit.Health.BonusHealth = unit.Attributes.GetMaxHealthBonus();
                unit.Composure.BonusComposure = unit.Attributes.GetMaxComposureBonus();

                AttributeModifcation movespeedBonus = finalModification.Movespeed;
                Positionable positionable = _positionableRepository.Get(target.Id);
                positionable.Speed = (unit.Actor.BaseMovespeed + unit.Attributes.GetSwiftnessMovespeedBonus() + movespeedBonus.BaseValue) * (1 + movespeedBonus.Percent / 100) + movespeedBonus.BonusValue;
                _positionableRepository.Update(positionable);
            }

            return targets;
        }
    }
}
