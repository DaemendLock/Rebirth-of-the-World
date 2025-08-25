using UnityEngine;
using UnityEngine.UI;

using UtilsUnity.Patterns.View;

namespace Client.Combat.Presentation.UI.ActionBar
{
    public class SpellCard : MonoBehaviour//, IBindableView<Ability>
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private Image _cooldown;

        //private Ability _model;

        private void Update()
        {
            //_cooldown.fillAmount = _model.Cooldown.Expired ? 0 : _model.Cooldown.Left / _model.Cooldown.FullTime;
        }

        //public void Bind(Ability value)
        //{
        //    if (value == null)
        //    {
        //        gameObject.SetActive(false);
        //        return;
        //    }

        //    _model = value;
        //    _abilityIcon.sprite = _model.Icon;
        //    _cooldown.sprite = _model.Icon;
        //    gameObject.SetActive(true);
        //}
    }
}
