using Server.Combat.Domain.Entities;
using Server.Combat.Infrastructure.Factories;
using Server.Combat.Infrastructure.Repositories;
using Server.Combat.Infrastructure.Services;

namespace Server.Combat.Infrastructure.Implementations.Services
{
    public class SkillCastService : ISkillCastService
    {
        private readonly ISkillScriptFactory _skillScriptFactory;
        private readonly IActionHandlerRepository _activeHandlerRepository;

        public SkillCastService(ISkillScriptFactory skillScriptFactory, IActionHandlerRepository activeHandlerRepository)
        {
            _skillScriptFactory = skillScriptFactory;
            _activeHandlerRepository = activeHandlerRepository;
            //_frameDataRepository = frameDataRepository;
        }

        public bool CanCast(Skill skill, Unit caster) => caster.GetCooldown(skill.Id) <= 0;

        public void Cast(Skill skill, Unit caster)
        {
            //CastHandler result = new(_skillScriptFactory.Create(skill, caster), skill.FrameData);

            //result.ActiveTime = 0;

            //if (skill.Flags.HasFlag(SkillFlags.Instant))
            //{
            //    return;
            //}

            //_activeHandlerRepository.Add(result);
        }

        //private bool AllowCast(IActionHandler value) => value is not CastHandler ability;
    }
}
