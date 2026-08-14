using Combat.API;
using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class DomainEncounterContext : IEncounterContext
    {
        private readonly EncounterFacade _sceneFacade;
        private readonly CharacterApiAdapter _chracterApiAdapter;

        public DomainEncounterContext(EncounterFacade sceneFacade, CharacterApiAdapter chracterApiAdapter)
        {
            _sceneFacade = sceneFacade;
            _chracterApiAdapter = chracterApiAdapter;
        }

        public object CreateProjectile(object from, object speed, IHitHandler hitHandler)
        {
            throw new System.NotImplementedException();
        }

        public Unit CreateUnit(CreateUnitInfo data)
        {
            CreateCharacterDTO dto = new(data.ModelName, data.Team, data.Position, -1, data.BaseHealth, data.Attributes, System.Array.Empty<ResourceValue>(), System.Array.Empty<SkillId>());
            return _chracterApiAdapter.Adaptee(_sceneFacade.CreateUnit(dto));
        }

        public void CreateStatus(ApplyStatusInfo info)
        {
            ApplStatusDTO dto = new(info.Target.Id, info.Name, info.Duration, info.StackCount, info.Source?.AbilityKey);
            _sceneFacade.CreateStatus(dto);
        }

        public Unit[] FindUnitsInRadius(UnityEngine.Vector3 center, float radius)
        {
            var ids = _sceneFacade.FindCharactersInRadius(center, radius);
            Unit[] result = new Unit[ids.Count];

            int index = 0;

            foreach (var id in ids)
            {
                result[index++] = _chracterApiAdapter.Adaptee(id);
            }

            return result;
        }

        public void RemoveUnit(UnitId unitId) => _sceneFacade.RemoveUnit(unitId);

        public void End()
        {

        }
    }
}
