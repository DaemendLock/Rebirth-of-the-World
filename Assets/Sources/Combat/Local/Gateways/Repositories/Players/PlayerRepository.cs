using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Players
{
    public sealed class PlayerRepository : IPlayerRepository
    {
        private readonly Dictionary<PlayerId, PlayerModelComponent> _values;
        private readonly ISceneObjectDataSource _sceneObjectDataSource;

        public PlayerRepository(ISceneObjectDataSource sceneObjectDataSource)
        {
            _sceneObjectDataSource = sceneObjectDataSource;

            _values = new();
        }

        public void Create(Player player)
        {
            Camera camera = Camera.main;
            PlayerModelComponent playerModel = camera.gameObject.AddComponent<PlayerModelComponent>();

            UpdateFollow(playerModel, player.ControlledEntity);
            _values.Add(player.Id, playerModel);
        }

        public void Delete(PlayerId id)
        {
            if (_values.TryGetValue(id, out var value))
            {
                UnityEngine.Object.Destroy(value.gameObject);
            }

            _values.Remove(id);
        }

        public Player Get(PlayerId id)
        {
            if (_values.TryGetValue(id, out var model) == false)
            {
                throw new InvalidOperationException();
            }

            return new(id, model.FollowId);
        }

        public void Update(Player player)
        {
            UpdateFollow(_values[player.Id], player.ControlledEntity);
        }

        public bool TryFindOwner(UnitId entityId, out Player player)
        {
            foreach (var component in _values)
            {
                if (component.Value.FollowId.HasValue && component.Value.FollowId.Value == entityId)
                {
                    player = new(component.Key, entityId);
                    return true;
                }
            }

            player = default;
            return false;
        }

        private void UpdateFollow(PlayerModelComponent model, UnitId? id)
        {
            if (id.HasValue == false)
            {
                model.Follow(null);
                model.FollowId = id;
                return;
            }

            if (_sceneObjectDataSource.TryGet(id.Value, out var val) == false)
            {
                return;
            }

            model.Follow(val.transform);
            model.FollowId = id;
        }
    }
}
