using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class CastSkillUseCase
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ICastSkillEventHandler _castSkillEventHandler;

        public CastFailReason Execute(SkillId skillId, EntityId? caster)
        {
            Skill skill = _skillRepository.Get(skillId, caster);

            if (skill.CanCast == false)
            {
                return CastFailReason.NotCastable;
            }

            if (caster.HasValue)
            {
                Actor actor = _actorRepository.Get(caster.Value);

                if (actor.CanCast == false)
                {
                    return CastFailReason.CantCast;
                }
            }

            _castSkillEventHandler.HandleEvent(caster, skillId);
            return CastFailReason.Success;
        }
    }

    public class CastSkillFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;

        private readonly ICastSkillEventHandler _castSkillEventHandler;

        private readonly ICastOutput _castOutput;

        public CastSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, ICastSkillEventHandler castSkillEventHandler/*, ISkillRepository skillRepository*/, ICastOutput castOutput)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _castSkillEventHandler = castSkillEventHandler;
            _castOutput = castOutput;
            //_skillRepository = skillRepository;
        }

        public CastFailReason Execute(EntityId caster, int slot)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(caster);

            SkillId? skillId = skillOwner.GetSkill(slot);

            if (skillId.HasValue == false)
            {
                Debug.Log("No skill in slot");
                return CastFailReason.UnknownSkill;
            }

            //Skill skill = _skillRepository.Get(skillId.Value, caster);

            //if (skill.CanCast == false)
            //{
            //    return CastFailReason.NotCastable;
            //}

            //Actor actor = _actorRepository.Get(caster);

            //if (actor.CanCast == false)
            //{
            //    return CastFailReason.CantCast;
            //}

            _castOutput.PlayAnimation(caster, skillId.Value);
            return _castSkillEventHandler.HandleEvent(caster, skillId.Value);
        }
    }

    public interface ICastSkillEventHandler
    {
        CastFailReason HandleEvent(EntityId? caster, SkillId skill);
    }

    public interface ICastOutput
    {
        void PlayAnimation(EntityId id, SkillId skill);
    }
}
