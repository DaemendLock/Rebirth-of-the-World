using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public interface ICharacterViewContainer
    {
        void Save(EntityId entityId, Transform value);
        Transform Get(EntityId entityId);
        bool TryGetValue(EntityId entityId, out Transform result);
    }

    public class UnitPresenter : IHealthOutput, IGiveResourceOutput, ISpendResourceOutput, IMovementOutput, ICastOutput
    {
        private readonly ICharacterViewContainer _container;
        private readonly ISkillAnimationRepository _skillAnimationRepository;

        public UnitPresenter(ICharacterViewContainer container, ISkillAnimationRepository skillAnimationRepository)
        {
            _container = container;
            _skillAnimationRepository = skillAnimationRepository;
        }

        public void PlayAnimation(EntityId target, SkillId skill)
        {
            AnimationClip clip = _skillAnimationRepository.Get(skill);

            if (_container.TryGetValue(target, out var view) == false)
            {
                return;
            }

            ActivityViewModel skillViewModel = new(clip, 0, 0);

            view.GetComponent<CasterView>().DisplayAction(skillViewModel);

        }

        void IGiveResourceOutput.Present(Resource value)
        {
            Debug.Log($"Resource update for {value.Id}: {value.CurrentValue}/{value.MaxValue}");
        }

        void ISpendResourceOutput.Present(Resource value)
        {

            Debug.Log($"Resource update for {value.Id}: {value.CurrentValue}/{value.MaxValue}");
        }

        public void Present(Health value)
        {
            Debug.Log($"Health updated for {value.Id}: {value.CurrentHealth}/{value.MaxHealth}");
        }

        public void SetHorizontalVelocity(EntityId target, Vector3 velocity)
        {
            if (_container.TryGetValue(target, out var view) == false)
            {
                return;
            }

            Rigidbody rigidbody = view.GetComponent<Rigidbody>();
            rigidbody.linearVelocity = new(velocity.x, rigidbody.linearVelocity.y, velocity.z);
        }
    }
}