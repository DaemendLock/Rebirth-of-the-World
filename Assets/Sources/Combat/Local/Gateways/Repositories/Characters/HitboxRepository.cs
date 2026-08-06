using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using HitboxInfo = Data.Entities.Components.Hitbox;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public readonly struct HitboxOwnerData
    {
        public readonly List<HitboxModelComponent> Hitboxes;
    }

    public class HitboxRepository : IHitboxOwnerRepository
    {
        private readonly Dictionary<UnitId, List<HitboxModelComponent>> _values;
        private readonly ISceneObjectDataSource _sceneObjectDataSource;

        public HitboxRepository(ISceneObjectDataSource sceneObjectDataSource)
        {
            _sceneObjectDataSource = sceneObjectDataSource;
            _values = new();
        }

        public void Create(UnitId id)
        {
            if (_sceneObjectDataSource.TryGet(id, out Transform model) == false)
            {
                return;
            }

            HitboxInfo[] hitboxes = model.GetComponentsInChildren<HitboxInfo>();

            List<HitboxModelComponent> result = new(hitboxes.Length);
            _values.Add(id, result);

            foreach (HitboxInfo hitboxData in hitboxes)
            {
                var value = hitboxData.gameObject.AddComponent<HitboxModelComponent>();
                value.Owner = id;
                value.Type = hitboxData.Type;
                result.Add(value);

                UnityEngine.Object.Destroy(hitboxData);
            }
        }

        public void Delete(UnitId id)
        {
            if (_values.TryGetValue(id, out var models) == false)
            {
                return;
            }

            foreach (var value in models)
            {
                UnityEngine.Object.Destroy(value);
            }

            _values.Remove(id);
        }

        public IEnumerable<Queue<HitRecord>> GetHits(UnitId id) => _values[id].Select(value => value.Records);
    }
}
