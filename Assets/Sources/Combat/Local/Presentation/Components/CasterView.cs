using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Combat.Local.Presentation.Components
{
    [RequireComponent(typeof(DaeAnimator.CharacterAnimator))]
    public class CasterView : MonoBehaviour
    {
        private const string AnimatorIsCastingName = "Casting";
        private const string AnimatorActiveSkillName = "ActiveSkill";

        //[Zenject.Inject] private ISkillDataRepository _skillDataRepository;

        private DaeAnimator.CharacterAnimator _daeAnimator;

        private ActivityViewModel _activeAction;

        private void Awake()
        {
            _daeAnimator = GetComponent<DaeAnimator.CharacterAnimator>();
        }

        public bool Casting => _activeAction != null;

        public void DisplayAction(ActivityViewModel action)
        {
            if (action == null || action.Clip == null)
            {
                return;
            }

            _activeAction = action;

            _daeAnimator.Play(new(action.Clip, action.StartTime, action.RecoveryTime));
        }
    }
}
