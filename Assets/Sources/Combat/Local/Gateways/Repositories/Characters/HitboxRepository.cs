using Combat.Common.Primitives;
using Combat.Local.Application.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

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
        private readonly IHitRecordQueue _hitRecordQueue;

        public HitboxRepository(ISceneObjectDataSource sceneObjectDataSource, IHitRecordQueue hitRecordQueue)
        {
            _sceneObjectDataSource = sceneObjectDataSource;
            _values = new();
            _hitRecordQueue = hitRecordQueue;
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
                HitboxModelComponent value = hitboxData.gameObject.AddComponent<HitboxModelComponent>();
                value.Owner = id;
                value.Type = hitboxData.Type;
                value.Hitted += _hitRecordQueue.Enqueue;
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

            foreach (HitboxModelComponent value in models)
            {
                UnityEngine.Object.Destroy(value);
            }

            _values.Remove(id);
        }
    }
}
