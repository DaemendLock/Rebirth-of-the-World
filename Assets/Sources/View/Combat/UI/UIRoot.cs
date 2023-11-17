using Core.Combat.Units;
using Input;
using UnityEngine;
using View.Combat.UI.Elements;
using View.Combat.UI.Nameplates.Elemets;

namespace View.Combat.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private ActionBar.ActionBar _actionBar;
        [SerializeField] private ResourceBar.ResourceBar _resourceBar;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CastBar _castBar;

        public static UIRoot Instance { get; private set; }

        public Unit Selection { get; private set; }

        private void Start()
        {
            if(Instance != null)
            {
                enabled = false;
            }

            Instance = this;
        }


        private void OnEnable()
        {
            SellectionInfo.SelectionChanged += DisplayUnit;
        }

        private void OnDisable()
        {
            SellectionInfo.SelectionChanged -= DisplayUnit;
        }

        private void Update()
        {
            if (Selection == null)
                return;

            if(_castBar.enabled)
            {
                //_castBar.SetValue();
            }
        }

        private void DisplayUnit(int id)
        {
            Selection = Core.Combat.Engine.Combat.GetUnit(id);

            if (Selection == null)
            {
                return;
            }

            _actionBar.AssignTo(Selection);
            _healthBar.AssignTo(Selection);
            _resourceBar.AssignTo(Selection);
            _castBar.AssignTo(Selection);

            //Selection.GetCastTime();
        }
    }
}
