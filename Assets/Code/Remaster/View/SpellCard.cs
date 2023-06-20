using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Remaster.View
{
    public class SpellCard : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Ability _spell;

        public void Init(Ability spell)
        {
            _spell = spell;
        }

        public void UpdateCard()
        {
            if(_spell == null)
            {
                return;
            }

            _slider.value = _spell.Cooldown.Left;
            _slider.maxValue = _spell.Cooldown.FullTime;
        }
    }
}
