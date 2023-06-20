using Combat.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class StatusIcon : RefreshableUI
    {

        [SerializeField] private Image _statusIcon;
        [SerializeField] private Image _durationIcon;
        [SerializeField] private TextMeshProUGUI _duarionText;
        [SerializeField] private LayoutElement _sizeControler;

        private Status _representingStatus;
        private int _size;
        public void BindStatus(Status s, int size)
        {
            if (_representingStatus != null)
                return;

            _representingStatus = s;
            _statusIcon.sprite = s.GetIcon();
            _durationIcon.fillAmount = s.GetFullDuration() > 0 ? s.GetRemainingTime() / s.Duration : 0;
            s.Destroied += OnExpired;
            SetIconSize(size);
            gameObject.SetActive(true);
        }

        private void Start()
        {
            if (_representingStatus == null)
                gameObject.SetActive(false);
        }

        public void SetIconSize(int size)
        {
            _size = size;
            UpdateUIData();
        }

        private void OnExpired()
        {
            Destroy(gameObject);
        }

        private void OnDurationUpdated() => _durationIcon.fillAmount = _representingStatus.GetFullDuration() > 0 ? _representingStatus.GetRemainingTime() / _representingStatus.Duration : 0;

        private void LateUpdate()
        {
            OnDurationUpdated();
        }

        private void OnDestroy()
        {

        }

        public override void UpdateUIData()
        {
            _size = _representingStatus.IsDebuff() ? Lobby.ActiveAccount.Data.Settings.Nameplates.DebuffIconSize : Lobby.ActiveAccount.Data.Settings.Nameplates.BuffIconSize;
            _sizeControler.flexibleWidth = _size;
        }

        public override void Hide()
        {
        }

        public override void Show()
        {
        }
    }
}
