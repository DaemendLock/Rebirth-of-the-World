using Combat.Local.Factories;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Common.ValueObjects;

namespace Testing.Local.Temp.Factories
{
    public class UnitModelFactory : IUnitModelFactory
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IKillableRepository _killableRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceRepository _resourceRepository;

        private int _nextId = 0;

        public UnitModelFactory(IHealthRepository healthRepository,
                IKillableRepository killableRepository,
                IPositionRepository positionRepository,
                IAttributesRepository attributesRepository,
                IResourceRepository resourceRepository)
        {
            _healthRepository = healthRepository;
            _killableRepository = killableRepository;
            _positionRepository = positionRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
        }

        public EntityId Create(IUnitControllerFactory.UnitModelCreationData context)
        {
            EntityId id = new(_nextId++);

            Killable killable = new(id, true);

            Health health = new(id, context.BaseHealth)
            {
                CurrentHealth = context.BaseHealth,
            };

            Transform positionable = new(id, context.UnitId, context.Team)
            {
                Position = context.Position
            };

            AttributeValue[] attributeValue = new AttributeValue[Attributes.AttributeCount];

            for (int i = 0; i < Attributes.AttributeCount; i++)
            {
                attributeValue[i] = context.DefaultAttributes[(Attribute) i];
            }

            Attributes attributes = new(id, attributeValue);

            _killableRepository.Create(killable);
            _healthRepository.Create(health);
            _positionRepository.Create(positionable);
            _attributesRepository.Create(attributes);
            _resourceRepository.Create(new(id, new(2), 100, 0));

            return id;
        }
    }
}
