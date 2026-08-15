using JetBrains.Annotations;

using UnityEngine;

namespace Client.Testing.View
{
    public class TestMenu : MonoBehaviour
    {
        [Zenject.Inject] private ITestMenuStrategy _menuStrategy;

        [SerializeField] private bool _holdToShow;
        [SerializeField] private KeyCode _showButton;

        [SerializeField] private InfoText _infoText;
        [SerializeField] private GameObject _menu;
        private bool _show;

        private void Start()
        {
            Show = false;
            _infoText.KeyCode = _showButton;
        }

        private void Update()
        {
            if (_holdToShow && Show != Input.GetKey(_showButton))
            {
                Show = Input.GetKey(_showButton);
                return;
            }

            if (Input.GetKeyUp(_showButton))
            {
                Show = !Show;
            }
        }

        private bool Show
        {
            get => _show;
            set
            {
                _show = value;

                _menu.SetActive(_show);
                _infoText.gameObject.SetActive(!_show);
            }
        }

        [UsedImplicitly]
        public void Kill() => _menuStrategy.Kill();

        [UsedImplicitly]
        public void Resurrect() => _menuStrategy.Resurrect();

        [UsedImplicitly]
        public void TakeDamage() => _menuStrategy.TakeDamage();

        [UsedImplicitly]
        public void HalfHealth() => _menuStrategy.HalfHealth();

        [UsedImplicitly]
        public void HealHealth() => _menuStrategy.HealHealth();

        [UsedImplicitly]
        public void SelectUnit() => _menuStrategy.Select();

        [UsedImplicitly]
        public void Leave() => _menuStrategy.Leave();
    }
}