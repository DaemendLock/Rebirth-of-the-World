using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Facades;

namespace Combat.API
{
    public sealed class SceneApi
    {
        private readonly SceneFacade _sceneFacade;

        public SceneApi(SceneFacade sceneFacade)
        {
            _sceneFacade = sceneFacade;
        }

        public object CreateProjectile(object from, object speed, IHitHandler hitHandler)
        {
            throw new System.NotImplementedException();
        }

        public void CreateUnit(CreateUnitInfo data)
        {
            CreateCharacterDTO dto = new(data.ModelName, data.Team, data.Position, -1, data.BaseHealth, data.Attributes, System.Array.Empty<SkillId>());
            _sceneFacade.CreateUnit(dto);
        }

        public void CreateStatus(ApplyStatusInfo info)
        {
            ApplStatusDTO dto = new(info.Target.Id, info.Name, info.Duration, info.StackCount, info.Source?.SkillId, info.Source?.Owner?.Id);
            _sceneFacade.CreateStatus(dto);
        }
    }
}
