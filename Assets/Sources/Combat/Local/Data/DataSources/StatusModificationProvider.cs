using Combat.Common.ValueObjects;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Data.Databases
{
    public class StatusModificationProvider : IStatusApiDataSource
    {
        private readonly Dictionary<EntityId, float[]> _cachedAttributes;

        private readonly Stack<float[]> _attributeArrayPool;

        public StatusModificationProvider()
        {
            //_cachedStates = new();
            _cachedAttributes = new();

            _attributeArrayPool = new();
        }

        public void ClearCache()
        {
            ClearAttributesCache();
            //_cachedStates.Clear();
        }

        public bool RestrictMovement(EntityId entityId) => false;

        private void ClearAttributesCache()
        {
            foreach (float[] attributeValue in _cachedAttributes.Values)
            {
                _attributeArrayPool.Push(attributeValue);
            }

            _cachedAttributes.Clear();
        }
    }
}
