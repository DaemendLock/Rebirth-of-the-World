using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public class UnitCreateUseCase
    {
        private readonly UnitIdFactory _idFactory;
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly IScaleEffectOwnerRepository _scaleEffectOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        private readonly ICharacterCreateOutput _outputPort;

        public UnitCreateUseCase(
            IUnitStore unitStore,
            IPositionableRepository positionableRepository, IActorRepository actorRepository,
            ICharacterCreateOutput outputPort,
            ISkillLyfecycleHandler skillLyfecycleHandler, ICharacterUpdateRepository characterUpdateList,
            IMovementEffectOwnerRepository movementEffectOwnerRepository,
            IHitboxOwnerRepository hitboxOwnerRepository, IHurtableRepository hurtableRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository)

        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;

            _idFactory = new(characterUpdateList);
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _characterUpdateList = characterUpdateList;
            _outputPort = outputPort;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _hurtableRepository = hurtableRepository;
            _scaleEffectOwnerRepository = scaleEffectOwnerRepository;
        }

        public void Execute(UnitId id, UnitCreationInfo context)
        {
            if (_characterUpdateList.TryGet(id, out _))
            {
                throw new System.InvalidOperationException($"Character with Id {id} alreday exists");
            }

            UnitContext unit = new(id);
            _positionableRepository.Create(CreatePositionable(id, context));
            unit.Attributes = CreateAttributesOwner(id, context);

            UnsortedCreate(ref unit, context);

            float currentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth;
            float currentComposure = context.InitialComposure < 0 ? context.DefaultComposure : context.InitialComposure;
            unit.Health = new(id, currentHealth, context.DefaultHealth);
            unit.Composure = new(id, currentComposure, context.DefaultComposure);
            SkillOwner skillOwner = CreateSkillOwner(id, context);
            unit.Skills = skillOwner;
            unit.Statuses = new(id);
            _unitStore.Create(unit);

            foreach (SkillId skillId in skillOwner.Skills)
            {
                _skillLyfecycleHandler.Give(new(id, skillId));
            }

            _outputPort.Present(id);
        }

        public UnitId Execute(UnitCreationInfo context)
        {
            UnitId id = _idFactory.GetId();
            Execute(id, context);
            return id;
        }

        private Positionable CreatePositionable(UnitId id, UnitCreationInfo context) => new(id, context.Position, default, 1f, default, context.Model, Vector3.zero, context.Team);

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

        private void UnsortedCreate(ref UnitContext unit, UnitCreationInfo context)
        {
            UnitId id = unit.Id;
            unit.Resources = new(id, context.DefaultResources.ToArray());
            _actorRepository.Create(new(id, ActorState.None, null, ConsciousState.Alive, new(default, System.Numerics.Vector2.Zero)));
            _hitboxOwnerRepository.Create(id);
            _hurtableRepository.Create(id);
            _characterUpdateList.Create(new(id, 1));
            _movementEffectOwnerRepository.Create(new(id, default));
            _scaleEffectOwnerRepository.Create(new(id));
        }
    }
}
