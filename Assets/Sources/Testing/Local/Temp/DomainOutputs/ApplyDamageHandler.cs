using Combat.API;
using Combat.API.Controllers;
using Combat.API.DTO;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class ApplyDamageHandler : IApplyDamageEventHandler
    {
        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly UnitEventApiController _eventApiController;

        public ApplyDamageHandler(UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider, UnitEventApiController eventApiController)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _eventApiController = eventApiController;
        }

        public void HandleEvent(DamageResult instance)
        {
            DamageRecord @event = CreateDamageRecord(instance);

            if (instance.Attacker.HasValue)
            {
                _eventApiController.HandleDamageDealth(instance.Attacker.Value, @event);
            }

            _eventApiController.HandleDamageRecived(instance.Target, @event);
        }

        private DamageRecord CreateDamageRecord(DamageResult instance)
        {
            EntityId targetId = instance.Target;
            Unit target = _unitApiProvider.Get(targetId);

            Unit attacker;
            SkillApi scriptedSkill;

            if (instance.Skill.HasValue)
            {
                scriptedSkill = _skillApiProvider.Get(instance.Skill.Value, instance.Caster);
            }
            else
            {
                scriptedSkill = null;
            }

            if (instance.Attacker.HasValue)
            {
                attacker = _unitApiProvider.Get(instance.Attacker.Value);
            }
            else
            {
                attacker = null;
            }

            return new(target, instance.OriginalDamage, instance.FinalDamage, instance.Flags, attacker, scriptedSkill);
        }
    }
}
