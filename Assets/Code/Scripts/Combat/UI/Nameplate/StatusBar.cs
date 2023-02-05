using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusBar : RefreshableUI {

    [SerializeField] private Unit unit;

    [SerializeField] private GameObject _buffsBox;
    [SerializeField] private GameObject _debuffsBox;

    [SerializeField] private GameObject _statusPrefab;

    private void Start() {
        unit.StatusApplied += AddNewStatus;
    }

    private void AddNewStatus(Status s) {
        Instantiate(_statusPrefab, (s.IsDebuff() ? _debuffsBox : _buffsBox).transform ).GetComponent<StatusIcon>().BindStatus(s);
    }

    public override void UpdateUIData() {
        
    }

    public override void Hide() {
        
    }

    public override void Show() {
        
    }
}
