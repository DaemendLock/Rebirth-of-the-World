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
        private readonly IUnitStore _unitStore;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IAttributesModificationCalculator _statusAttributeCalculator;

        public AttributeOwnerUpdateAllUseCase(IUnitStore unitStore, ICharacterUpdateRepository characterUpdateList, IAttributesModificationCalculator statusAttributeCalculator)
        {
            _unitStore = unitStore;
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
                if (_unitStore.TryGet(value, out UnitContext unit) == false)
                {
                    continue;
                }

                unit.Attributes.Clear();
            }

            foreach (UnitId value in values)
            {
                if (_unitStore.TryGet(value, out UnitContext unit) == false)
                {
                    continue;
                }

                AttributesBonuses finalModification = _statusAttributeCalculator.Evaluate(unit.Statuses.GetAll().Select(value => value.Id));

                for (int j = 0; j < unit.Attributes.BaseValues.Length; j++)
                {
                    unit.Attributes.SetModification((UnitAttribute)j, finalModification[(UnitAttribute)j]);
                }

                _characterUpdateList.Update(new(value, Math.Max(1 + (finalModification.TimeScale / 100f), 0f)));
                unit.Health.BonusHealth = unit.Attributes.GetMaxHealthBonus();
            }
        }
    }
}
