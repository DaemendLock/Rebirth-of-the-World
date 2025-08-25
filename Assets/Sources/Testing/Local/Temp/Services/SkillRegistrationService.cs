using Combat.Common.ValueObjects;
using Combat.Local.Data.Entities;
using Combat.Local.Data.Repositories;
using Combat.Local.Data.Services;
using Combat.Local.Domain.Repositories;

namespace Testing.Local.Temp.Services
{
    public class SkillRegistrationService : ISkillRegistrationService
    {
        private readonly ISkillAnimationRepository _skillAnimationRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillScriptNameRepository _skillScriptNameRepository;

        public SkillRegistrationService(ISkillAnimationRepository skillAnimationRepository, ISkillRepository skillRepository, ISkillScriptNameRepository skillScriptNameRepository)
        {
            _skillAnimationRepository = skillAnimationRepository;
            _skillRepository = skillRepository;
            _skillScriptNameRepository = skillScriptNameRepository;

            foreach (CastableSkillData data in UnityEngine.Resources.LoadAll<CastableSkillData>("Temp/TestSkills"))
            {
                Register(data);
            }
        }

        public void Register(ISkillData value)
        {
            SkillId id = value.Id;

            if (_skillRepository.Contains(id))
            {
                return;
            }

            if (value is CastableSkillData castableSkill)
            {
                _skillAnimationRepository.Create(id, castableSkill.AnimationClip);
            }

            _skillRepository.Create(new(id, value.Flags, value.FrameData));
            _skillScriptNameRepository.Add(id, value.ScriptName);
        }
    }
}
