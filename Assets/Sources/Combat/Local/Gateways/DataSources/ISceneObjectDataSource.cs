using Combat.Common.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISceneObjectDataSource
    {
        Transform GetCharacterTransform(EntityId id);

        bool TryGetCharacterTransform(EntityId id, out Transform transform);

        ICollection<EntityId> FindCharacterInRadius(Vector3 location, float radius);
    }
}
