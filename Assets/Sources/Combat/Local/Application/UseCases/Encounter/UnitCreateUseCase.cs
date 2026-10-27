using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public class UnitCreateUseCase
    {
        private readonly UnitIdFactory _idFactory;
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly ICharacterUpdateQueue _characterUpdateList;

        private readonly ICharacterCreateOutput _outputPort;
        private readonly ICharacterConsciousStateOutput _consciousStateOutput;

        public UnitCreateUseCase(
            IUnitStore unitStore, IPositionableRepository positionableRepository,
            ICharacterCreateOutput outputPort, ICharacterConsciousStateOutput consciousStateOutput,
            ISkillLyfecycleHandler skillLyfecycleHandler, ICharacterUpdateQueue characterUpdateList,
            IHitboxOwnerRepository hitboxOwnerRepository, IHurtableRepository hurtableRepository)

        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;

            _idFactory = new(unitStore);
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _characterUpdateList = characterUpdateList;
            _outputPort = outputPort;
            _consciousStateOutput = consciousStateOutput;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _hurtableRepository = hurtableRepository;
        }

        public void Execute(UnitId id, UnitCreationInfo context)
        {
            if (_unitStore.TryGet(id, out _))
            {
                throw new System.InvalidOperationException($"Character with Id {id} alreday exists");
            }

            UnitContext unit = CreateUnitContext(id, context);
            _unitStore.Create(unit);

            _positionableRepository.Create(CreatePositionable(id, context));

            UnsortedCreate(id);

            foreach (SkillId skillId in unit.Skills.Skills)
            {
                _skillLyfecycleHandler.Give(new(id, skillId));
            }

            _outputPort.Present(id);
            _consciousStateOutput.Present(id, unit.LifeState.State);
        }

        public UnitId Execute(UnitCreationInfo context)
        {
            UnitId id = _idFactory.GetId();
            Execute(id, context);
            return id;
        }

        private Positionable CreatePositionable(UnitId id, UnitCreationInfo context) =>
            new(id, context.Position, default, 1f, default, context.Model, Vector3.zero, context.Team, new(), new());

        private UnitContext CreateUnitContext(UnitId id, UnitCreationInfo context)
        {
            float currentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth;
            float currentComposure = context.InitialComposure < 0 ? context.DefaultComposure : context.InitialComposure;

            UnitContext unit = new(id)
            {
                Resources = new(id, context.DefaultResources.ToArray()),
                Actor = new(id, context.BaseMovespeed),
                Attributes = CreateAttributesOwner(id, context),
                Health = new(id, currentHealth, context.DefaultHealth),
                Composure = new(id, currentComposure, context.DefaultComposure),
                Skills = CreateSkillOwner(id, context),
                Statuses = new(id)
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

        private void UnsortedCreate(UnitId id)
        {
            _hitboxOwnerRepository.Create(id); //Uses view
            _hurtableRepository.Create(id); //Uses view
            _characterUpdateList.Add(id);
        }
    }
}
