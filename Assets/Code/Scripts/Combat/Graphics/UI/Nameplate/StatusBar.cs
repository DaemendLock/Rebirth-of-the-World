using Combat.Status;
using UnityEngine;

namespace UI.Combat
{
    public class StatusBar : RefreshableUI, IUnitBindable
    {

        [SerializeField] private Unit _unit;

        [SerializeField] private GameObject _buffsBox;
        [SerializeField] private GameObject _debuffsBox;

        [SerializeField] private GameObject _statusPrefab;

        private int _buffSize;
        private int _debuffSize;

        private void Awake()
        {
            transform.parent.GetComponent<Nameplate>().SubscribeForOwnerBinding(this);
            UpdateUIData();
        }

        public void BindUnit(Unit owner)
        {
            if (_unit != null)
                return;
            if (owner == null)
                return;
            _unit = owner;
            _unit.StatusApplied += AddNewStatus;
        }

        private void AddNewStatus(Status s)
        {
            Instantiate(_statusPrefab, (s.IsDebuff() ? _debuffsBox : _buffsBox).transform).GetComponent<StatusIcon>().BindStatus(s, s.IsDebuff() ? _debuffSize : _buffSize);
        }

        public override void UpdateUIData()
        {
            _buffSize = Lobby.ActiveAccount.Data.Settings.Nameplates.BuffIconSize;
            _debuffSize = Lobby.ActiveAccount.Data.Settings.Nameplates.DebuffIconSize;
        }

        public override void Hide()
        {

        }

        public override void Show()
        {

        }
    }
}