using Client.Combat.Domain.Actions;
using Client.Combat.Domain.Units.Components;
using Client.Combat.Presentation.Units.Components;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units
{
    [RequireComponent(typeof(DaeAnimator.UnitAnimator))]
    public class CasterView : BindableViewComponent<IActionOwner>, ICasterView
    {
        private const string AnimatorIsCastingName = "Casting";
        private const string AnimatorActiveSkillName = "ActiveSkill";

        //[Zenject.Inject] private ISkillDataRepository _skillDataRepository;

        private DaeAnimator.UnitAnimator _daeAnimator;

        private IActionHandler _activeAction;

        private void Awake()
        {
            _activeAction = null;
            _daeAnimator = GetComponent<DaeAnimator.UnitAnimator>();
        }

        private void LateUpdate()
        {
            if (Model == null)
            {
                return;
            }

            IActionHandler action = Model.ActiveAction;

            if (action != _activeAction)
            {
                DisplayAction(action);
                _activeAction = action;
            }
        }

        public bool Casting => _activeAction != null;

        public void DisplayAction(IActionHandler action)
        {
            if (action == null)
            {
                return;
            }

            //if (action.Actor != Model || action is not CastHandler ability)
            //{
            //    return;
            //}

            //int skillId = ability.Skill.Id;

            //ISkillData skillData = _skillDataRepository.Get(skillId);

            //if (skillData == null || skillData.AnimationClip == null)
            //{
            //    return;
            //}

            //_daeAnimator.Play(new(skillData.AnimationClip, 0, 0), ability.ActiveTime);
        }
    }
}
