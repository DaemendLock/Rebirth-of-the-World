using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Update(Positionable positionable);
        Positionable Get(UnitId entityId);
        void Delete(UnitId entityId);
    }
}
