using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Units.ViewModels;

namespace Combat.Local.Presentation.Implementations.Units
{
    public class CharacterModelPresenter
    {
        private readonly CharacterView _view;

        public CharacterModelPresenter(CharacterView view)
        {
            _view = view;
        }

        public ModelName UnitName { get; }

        public void Hide()
        {
            _view.gameObject.SetActive(false);
        }

        public void PlayAnimation(SkillId skill)
        {

        }

        public ICollection<HitboxViewModel> GetHitboxes() => _view.GetComponentsInChildren<HitboxViewModel>();

        public ICollection<HurtboxViewModel> GetHurtboxes() => _view.GetComponentsInChildren<HurtboxViewModel>();
    }
}