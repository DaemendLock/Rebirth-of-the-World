using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.API
{
    public sealed class EncounterApi
    {
        private readonly IEncounterContext _context;

        public EncounterApi(IEncounterContext conext)
        {
            _context = conext;
        }

        public IEncounterContext Context => _context;

        public object CreateProjectile(object from, object speed, IHitHandler hitHandler) => _context.CreateProjectile(from, speed, hitHandler);

        public void CreateStatus(ApplyStatusInfo info) => _context.CreateStatus(info);

        public Unit CreateUnit(CreateUnitInfo data) => _context.CreateUnit(data);

        public Unit[] FindUnitsInRadius(UnityEngine.Vector3 center, float radius) => _context.FindUnitsInRadius(center, radius);

        public void Finalize(EncounterState reason) => _context.Finalize(reason);
    }
}
