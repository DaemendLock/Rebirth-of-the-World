using Combat.API;
using Combat.API.Controllers;
using Combat.API.DTO;
using Combat.Local.Domain.OutputPorts;

namespace Testing.Local.Temp.DomainOutputs
{
    public class CharacterGiveResourceEventHandler : IGiveResourceEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly CharacterEventApiController _eventApiController;

        public CharacterGiveResourceEventHandler(SkillApiProvider skillApiProvider, CharacterEventApiController eventApiController)
        {
            _skillApiProvider = skillApiProvider;
            _eventApiController = eventApiController;
        }

        public void HandleEvent(GiveResourceResult result)
        {
            SkillApi skill = null;

            if (result.Skill.HasValue)
            {
                _skillApiProvider.Get(result.Skill.Value, result.Caster);
            }

            ResourceChangeRecord @event = new(result.Resource, skill, result.Value);
            _eventApiController.HandleResourceGained(result.Target, @event);
        }
    }
}
