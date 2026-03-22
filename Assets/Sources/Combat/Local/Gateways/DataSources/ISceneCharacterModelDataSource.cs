using Combat.Common.ValueObjects;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISceneCharacterModelDataSource
    {
        CharacterModel Create(EntityId id, ModelName name, Transform parent);
        void Destroy(EntityId id);
        bool TryGetCharacterModel(EntityId id, out CharacterModel transform);
        ICollection<EntityId> FindCharacterInRadius(Vector3 location, float radius);
    }
}
