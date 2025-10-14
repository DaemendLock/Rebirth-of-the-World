using Combat.API;
using Combat.API.Controllers;

using Combat.Common.ValueObjects;

using Combat.Local.Controllers;
using Combat.Local.Data.Repositories;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Lookup
{
    public interface IPrecachable
    {
        void ClearCache();
        void Precache();
    }

    public class UnitLookup
    {
        private readonly CharacterApiProvider _unitApiProvider;
        private readonly CombatController _combatController;

        public ICollection<Unit> FindUnitsInRadius(Vector3 position, float radius)
        {
            ICollection<EntityId> data = _combatController.FindCharactersInRadius(position, radius);
            List<Unit> result = new(data.Count);

            foreach (var item in data)
            {
                result.Add(_unitApiProvider.Get(item));
            }

            return result;
        }

        public int FindUnitsInRadiusNoAlloc(Vector3 position, float radius, Span<Unit> buffer)
        {
            ICollection<EntityId> values = _combatController.FindCharactersInRadius(position, radius);

            int result = Math.Min(buffer.Length, values.Count);

            int i = 0;

            foreach (var item in values)
            {
                if (i == result)
                {
                    break;
                }

                buffer[i] = _unitApiProvider.Get(item);
            }

            return result;
        }
    }

    public class StatusLookup : IPrecachable, IStatusLookupService
    {
        private readonly StatusApiProvider _statusApiProvider;
        private readonly StatusRepository _statusRepository;

        private readonly Stack<List<StatusApi>> _listPool;
        private readonly Dictionary<EntityId, List<StatusApi>> _cache;

        public StatusLookup(StatusApiProvider statusRepository)
        {
            _statusApiProvider = statusRepository;

            _listPool = new();
            _cache = new();

            Update();
        }

        public void Update()
        {
            ClearCache();
            Precache();
        }

        public void Precache()
        {
            //ICollection<StatusId> values = _statusRepository.GetAllIds();
            ICollection<StatusApi> values = _statusApiProvider.GetAll();

            foreach (var value in values)
            {
                ListItem(value);
            }
        }

        public ICollection<StatusApi> FindStatusesOnUnit(EntityId owner)
        {
            if (_cache.TryGetValue(owner, out var result) == false)
            {
                return Array.Empty<StatusApi>();
            }

            return result;
        }

        public void ClearCache()
        {
            foreach (List<StatusApi> value in _cache.Values)
            {
                value.Clear();
                _listPool.Push(value);
            }

            _cache.Clear();
        }

        private void ListItem(StatusApi status)
        {
            EntityId parentId = status.Parent.Id;

            if (_cache.TryGetValue(parentId, out List<StatusApi> target))
            {
                target.Add(status);
                return;
            }

            if (_listPool.TryPop(out target) == false)
            {
                target = new();
            }

            _cache[parentId] = target;
            target.Add(status);
        }
    }
}
