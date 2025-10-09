using Combat.API;
using Combat.API.Controllers;
using Combat.API.DTO;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

namespace Testing.Local.Temp.DomainOutputs
{
    public class SpendResourceEventHandler : ISpendResourceEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly UnitEventApiController _eventApiController;

        public SpendResourceEventHandler(SkillApiProvider skillApiProvider, UnitEventApiController eventApiController)
        {
            _skillApiProvider = skillApiProvider;
            _eventApiController = eventApiController;
        }

        public void HandleEvent(EntityId target, ResourceId resource, float value, EventSource source)
        {
            SkillApi skill = _skillApiProvider.Get(source.Skill.Value, source.Unit.Value);
            ResourceChangeRecord @event = new(resource, skill, value);
            _eventApiController.HandleResourceSpent(target, @event);
        }
    }
}
