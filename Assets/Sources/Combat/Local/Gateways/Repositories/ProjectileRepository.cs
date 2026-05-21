using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class ProjectileRepository : IProjectileRepository
    {
        private readonly Dictionary<ProjectileId, ProjectileModel> _values;
        private readonly IProjectileModelFactory _factory;

        public void Create(Projectile value)
        {
            ProjectileModel projectileModel = _factory.Create(value.ModelName);
            projectileModel.transform.position = value.Position;
            projectileModel.Owner = value.Owner;
            _values.Add(value.Id, projectileModel);
        }

        public void Delete(ProjectileId id)
        {
            if (_values.TryGetValue(id, out ProjectileModel data) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(data.gameObject);
            _values.Remove(id);
        }

        public Projectile Get(ProjectileId id)
        {
            if (_values.TryGetValue(id, out ProjectileModel data) == false)
            {
                throw new System.InvalidOperationException("No key present");
            }

            return new(id, data.Owner, default, data.transform.position, data.Speed);
        }
        public void Update(Projectile value)
        {
            if (_values.TryGetValue(value.Id, out ProjectileModel projectileModel) == false)
            {
                throw new System.InvalidOperationException("No key present");
            }

            projectileModel.transform.position = value.Position;
            projectileModel.Owner = value.Owner;
        }
    }
}
