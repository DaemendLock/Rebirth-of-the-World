using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.API.Contexts
{
    public interface IEncounterContext
    {
        object CreateProjectile(object from, object speed, IHitHandler hitHandler);
        void CreateStatus(ApplyStatusInfo info);
        Unit CreateUnit(CreateUnitInfo data);
        void RemoveUnit(UnitId unitId);
        Unit[] FindUnitsInRadius(Vector3 center, float radius);
    }
}
