using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Infrastructure.Controllers;
using Combat.Local.Infrastructure.Presenters;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Combat.Local.Presentation.Implementations.Units
{
    public class UnitPresenter : IUnitPresenter
    {
        private readonly HitController _hitController;
        private readonly UnitViewModel _viewModel;

        private CharacterView _characterView;

        private CasterView _casterView;
        //private readonly ISkillAnimationRepository _skillAnimationRepository;

        public UnitPresenter(UnitViewModel viewModel, HitController hitController)
        {
            _viewModel = viewModel;

            _hitController = hitController;

            _casterView = _viewModel.GetComponentInChildren<CasterView>();
        }

        public bool OnGround => throw new System.NotImplementedException();

        public Vector3 Position { get => _viewModel.Position; set => _viewModel.Position = value; }

        public Vector3 Velocity { get => _viewModel.Velocity; set => _viewModel.Velocity = value; }

        public void PlayAnimation(AnimationClip clip)
        {
            if (clip == null)
            {
                _viewModel.ActiveAction = null;
                return;
            }

            _viewModel.ActiveAction = new(clip, 0);

            ActivityViewModel skillViewModel = new(clip, 0, 0);

            _casterView.DisplayAction(skillViewModel);

        }

        public void SetModel(CharacterView characterView)
        {
            if (_characterView != null)
            {
                foreach (var hitbox in _characterView.GetComponentsInChildren<HitboxViewModel>())
                {
                    _hitController.RemoveHitbox(hitbox.Id);
                }

                foreach (var hurtbox in _characterView.GetComponentsInChildren<HurtboxViewModel>())
                {
                    _hitController.RemoveHurtbox(hurtbox.Id);
                }
            }

            _characterView = characterView;
            EntityId owner = _viewModel.EntityId;

            foreach (var hitbox in _characterView.GetComponentsInChildren<HitboxViewModel>())
            {
                _hitController.AddHitbox(hitbox, owner);
            }

            foreach (var hurtbox in _characterView.GetComponentsInChildren<HurtboxViewModel>())
            {
                _hitController.AddHurtbox(hurtbox, owner);
            }
        }
    }
}