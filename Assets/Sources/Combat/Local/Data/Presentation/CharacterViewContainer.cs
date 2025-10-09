using Combat.Common.ValueObjects;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Presenters;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Presentation
{
    public class CharacterViewContainer : ICharacterViewContainer, ISceneObjectDataSource
    {
        private readonly Dictionary<EntityId, Transform> _values;

        public CharacterViewContainer()
        {
            _values = new();
        }

        public Transform Get(EntityId entityId) => _values[entityId];

        public Transform GetCharacterTransform(EntityId id) => Get(id);

        public void Save(EntityId entityId, Transform transform) => _values[entityId] = transform;

        public bool TryGetCharacterTransform(EntityId id, out Transform transform) => _values.TryGetValue(id, out transform);

        public bool TryGetValue(EntityId entityId, out Transform result) => _values.TryGetValue(entityId, out result);

        public ICollection<EntityId> FindCharacterInRadius(Vector3 origin, float radius)
        {
            Collider[] values = Physics.OverlapSphere(origin, radius, LayerMask.GetMask("Units"));
            List<EntityId> result = new(values.Length);

            foreach (Collider collider in values)
            {
                if (collider.attachedRigidbody.TryGetComponent(out CharacterView view))
                {
                    result.Add(view.Id);
                }
            }

            return result;
        }

        public int FindCharactersInRadiusNonAlloc(Vector3 position, float radius, Span<EntityId> buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
