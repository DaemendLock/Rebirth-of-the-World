using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

using HurtboxInfo = Data.Entities.Components.Hurtbox;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class HurtableRepository : IHurtableRepository
    {
        private readonly Dictionary<UnitId, List<HurtboxModelComponent>> _values;

        private readonly ISceneObjectDataSource _sceneObjectDataSource;

        public HurtableRepository(ISceneObjectDataSource sceneObjectDataSource)
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

            HurtboxInfo[] hurtboxes = model.GetComponentsInChildren<HurtboxInfo>();
            List<HurtboxModelComponent> result = new();
            _values.Add(id, result);

            foreach (HurtboxInfo hurtboxData in hurtboxes)
            {
                var value = hurtboxData.gameObject.AddComponent<HurtboxModelComponent>();
                value.Type = hurtboxData.Type;
                value.Owner = id;
                result.Add(value);

                UnityEngine.Object.Destroy(hurtboxData);
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
    }
}
