using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct GiveSkillUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IGiveSkillEventHandler _eventHandler;

        public GiveSkillUseCase(ISkillOwnerRepository skillOwnerRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _eventHandler = null;
        }

        public void GiveSkill(EntityId target, SkillId skillId)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(target);
            ReadOnlySpan<SkillId> oldSkills = skillOwner.GetAll();

            Span<SkillId> skills = stackalloc SkillId[oldSkills.Length + 1];
            oldSkills.CopyTo(skills);
            skills[^1] = skillId;
            _skillOwnerRepository.Update(new(target, skills));

            _eventHandler?.HandleEvent(target, skillId);
        }
    }

    public interface IGiveSkillEventHandler
    {
        void HandleEvent(EntityId target, SkillId skill);
    }
}
