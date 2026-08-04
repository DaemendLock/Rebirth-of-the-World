using Combat.API.DTO;
using Combat.API.Skills;

using UnityEngine;

namespace Combat.API
{
    public interface IEncounterApi
    {
        object CreateProjectile(object from, object speed, IHitHandler hitHandler);
        void CreateStatus(ApplyStatusInfo info);
        IUnit CreateUnit(CreateUnitInfo data);
        IUnit[] FindUnitsInRadius(Vector3 center, float radius);
    }
}