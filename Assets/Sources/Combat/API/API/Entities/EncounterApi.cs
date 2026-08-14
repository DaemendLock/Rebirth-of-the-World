using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.API
{
    public sealed class EncounterApi
    {
        private readonly IEncounterContext _conext;

        public EncounterApi(IEncounterContext conext)
        {
            _conext = conext;
        }

        public object CreateProjectile(object from, object speed, IHitHandler hitHandler) => _conext.CreateProjectile(from, speed, hitHandler);

        public void CreateStatus(ApplyStatusInfo info) => _conext.CreateStatus(info);

        public Unit CreateUnit(CreateUnitInfo data) => _conext.CreateUnit(data);

        public Unit[] FindUnitsInRadius(UnityEngine.Vector3 center, float radius) => _conext.FindUnitsInRadius(center, radius);

        public void Finalize(EncounterState reason) => _conext.Finalize(reason);
    }
}
