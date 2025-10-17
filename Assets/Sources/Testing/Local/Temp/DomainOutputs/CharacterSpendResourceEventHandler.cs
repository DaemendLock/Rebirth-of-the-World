using Combat.API;
using Combat.API.Controllers;
using Combat.API.DTO;
using Combat.Local.Domain.OutputPorts;

namespace Testing.Local.Temp.DomainOutputs
{
    public class CharacterSpendResourceEventHandler : ISpendResourceEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly CharacterEventApiController _eventApiController;

        public CharacterSpendResourceEventHandler(SkillApiProvider skillApiProvider, CharacterEventApiController eventApiController)
        {
            _skillApiProvider = skillApiProvider;
            _eventApiController = eventApiController;
        }

        public void HandleEvent(SpendResourceResult result)
        {
            SkillApi skill = result.Skill.HasValue ? _skillApiProvider.Get(result.Skill.Value, result.Caster) : null;
            ResourceChangeRecord @event = new(result.Resource, skill, result.Value);
            _eventApiController.HandleResourceSpent(result.Target, @event);
        }
    }
}
