using Combat.API.DTO;
using Combat.API.Skills;

using System.Numerics;

namespace Combat.API.Contexts
{
    public interface IEncounterContext
    {
        object CreateProjectile(object from, object speed, IHitHandler hitHandler);
        void CreateStatus(ApplyStatusInfo info);
        Unit CreateUnit(CreateUnitInfo data);
        Unit[] FindUnitsInRadius(Vector3 center, float radius);
    }
}
