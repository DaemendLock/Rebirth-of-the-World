using Combat.API;
using Combat.API.Controllers;
using Combat.API.DTO;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

namespace Testing.Local.Temp.DomainOutputs
{
    public class ApplyHealingHandler : IApplyHealingEventHandler
    {
        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly UnitEventApiController _eventApiController;

        public ApplyHealingHandler(UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider, UnitEventApiController eventApiController)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _eventApiController = eventApiController;
        }

        public void HandleEvent(HealingInstance instance)
        {
            HealingRecord @event = CreateHealingRecord(instance);

            if (instance.Healer.HasValue)
            {
                _eventApiController.HandleHealingDealth(instance.Healer.Value, @event);
            }

            _eventApiController.HandleHealingRecived(instance.Target, @event);
        }

        private HealingRecord CreateHealingRecord(HealingInstance instance)
        {
            EventSource source = instance.Source;

            Unit targetApi = _unitApiProvider.Get(instance.Target);

            Unit healerApi;
            SkillApi sourceSkill;

            if (instance.Healer.HasValue)
            {
                healerApi = _unitApiProvider.Get(source.Unit.Value);
            }
            else
            {
                healerApi = null;
            }

            if (source.Unit.HasValue)
            {
                sourceSkill = _skillApiProvider.Get(source.Skill.Value, source.Unit.Value);
            }
            else
            {
                sourceSkill = null;
            }

            return new(targetApi, healerApi, sourceSkill, instance.OriginalHealing, instance.Healing, instance.Flags);
        }
    }
}
