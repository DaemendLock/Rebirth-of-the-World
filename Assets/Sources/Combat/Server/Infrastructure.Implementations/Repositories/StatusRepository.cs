using System.Collections.Generic;
using System.Reflection;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Implementations.Statuses.Attributes;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Statuses.StatusEffects;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Domain.ValueObjects.Statuses;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        //private readonly IStatusFactory _statusFactory;
        private readonly IAttributesRepository _attributesRepository;

        private readonly Dictionary<int, StatusEffect> _statuses;
        private readonly Dictionary<int, AttributeModification> _attributes;

        private int _statusCount;

        public StatusRepository()
        {
            _statuses = new();
            _statusCount = 0;
            _attributes = new();
        }

        public void Update(float deltaTime)
        {
            foreach (StatusEffect effect in _statuses.Values)
            {
                //effect.ActiveTime += deltaTime;
            }

            foreach (AttributeModification attributeModification in _attributes.Values)
            {
                attributeModification.Evaluate();
            }
        }

        public StatusEffect Create(StatusApplicationData data)
        {
            return null;
        }

        public IEnumerable<StatusEffect> FindStatusEffects(EntityId id) => throw new System.NotImplementedException();

        public void Add(StatusEffect status)
        {
            if (status == null)
            {
                throw new System.ArgumentNullException(nameof(status));
            }

            _statuses[status.Id.Value] = status;

#if DEBUG
            StatusNameAttribute name = status.GetType().GetCustomAttribute<StatusNameAttribute>();

            if (name != null)
            {
                UnityEngine.Debug.Log($"Applied status \"{name.Name}\"");
            }
#endif
            if (status is IAttributesModification<Attribute> attributeBonus)
            {
                _attributes.Add(status.Id.Value, new(attributeBonus, _attributesRepository.Get(status.Parent.Id)));
            }
        }

        public void Remove(StatusId id)
        {
            if (_statuses.TryGetValue(id.Value, out StatusEffect removeStatus) == false)
            {
                return;
            }

            _statuses.Remove(id.Value);

            if (removeStatus is IAttributesModification<Attribute>)
            {
                _attributes.Remove(id.Value);
            }

            return;
        }

        public StatusEffect Get(StatusId id) => _statuses.GetValueOrDefault(id.Value, null);

        private int GetNextId()
        {
            int result = _statusCount++;

            while (_statuses.ContainsKey(result))
            {
                result++;
            }

            return result;
        }

        private readonly struct AttributeModification
        {
            private readonly IAttributesModification<Attribute> _attributesModification;
            private readonly IAttributeCollection<Attribute> _target;

            public AttributeModification(IAttributesModification<Attribute> attributesModification, IAttributeCollection<Attribute> target)
            {
                _attributesModification = attributesModification;
                _target = target;
            }

            public void Evaluate()
            {
                _attributesModification.Apply(_target);
            }
        }
    }
}
