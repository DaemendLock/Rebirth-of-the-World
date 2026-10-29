using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases
{
    public class UnitCreateUseCase
    {
        private readonly UnitIdFactory _idFactory;
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly ICharacterUpdateQueue _characterUpdateList;

        private readonly ICharacterCreateOutput _outputPort;
        private readonly ICharacterConsciousStateOutput _consciousStateOutput;

        public UnitCreateUseCase(
            IUnitStore unitStore, IPositionableRepository positionableRepository,
            ICharacterCreateOutput outputPort, ICharacterConsciousStateOutput consciousStateOutput,
            ISkillLyfecycleHandler skillLyfecycleHandler, ICharacterUpdateQueue characterUpdateList)

        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;

            _idFactory = new(unitStore);
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _characterUpdateList = characterUpdateList;
            _outputPort = outputPort;
            _consciousStateOutput = consciousStateOutput;
        }

        public void Execute(UnitId id, UnitCreationInfo context)
        {
            if (_unitStore.TryGet(id, out _))
            {
                throw new System.InvalidOperationException($"Character with Id {id} alreday exists");
            }

            UnitContext unit = CreateUnitContext(id, context);
            _unitStore.Create(unit);
            _characterUpdateList.Add(id);

            _positionableRepository.Create(unit.Positionable); //Uses view
            _outputPort.Present(id);
            _consciousStateOutput.Present(id, unit.LifeState.State);

            foreach (SkillId skillId in unit.Skills.Skills)
            {
                _skillLyfecycleHandler.Give(new(id, skillId));
            }
        }

        public UnitId Execute(UnitCreationInfo context)
        {
            UnitId id = _idFactory.GetId();
            Execute(id, context);
            return id;
        }

        private UnitContext CreateUnitContext(UnitId id, UnitCreationInfo context)
        {
            float currentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth;
            float currentComposure = context.InitialComposure < 0 ? context.DefaultComposure : context.InitialComposure;

            UnitContext unit = new(id)
            {
                Resources = new(id, context.DefaultResources.ToArray()),
                Actor = new(id, context.BaseMovespeed),
                Positionable = new(id, context.Position, default, 1f, default, context.Model, context.Team),
                Attributes = CreateAttributesOwner(id, context),
                Health = new(id, currentHealth, context.DefaultHealth),
                Composure = new(id, currentComposure, context.DefaultComposure),
                Skills = CreateSkillOwner(id, context),
                Statuses = new(id),
            };

            return unit;
        }

        private SkillOwner CreateSkillOwner(UnitId id, UnitCreationInfo context)
        {
            SkillOwner skillOwner = new(id);

            foreach (var item in context.Skills)
            {
                skillOwner.GiveSkill(item);
            }

            return skillOwner;
        }

        private AttributesOwner CreateAttributesOwner(UnitId id, UnitCreationInfo context)
        {
            AttributeValue[] attributeValue = new AttributeValue[AttributesOwner.AttributeCount];

            for (int i = 0; i < AttributesOwner.AttributeCount; i++)
            {
                if (i < context.DefaultAttributes.Length)
                {
                    attributeValue[i] = context.DefaultAttributes[i];
                }
            }

            return new(id, attributeValue);
        }
    }
}
