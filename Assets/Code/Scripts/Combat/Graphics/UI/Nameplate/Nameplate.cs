using System;
using System.Collections.Generic;
using UnityEngine;

public class Nameplate : RefreshableUI {
    [SerializeField] private GameObject _selectionFrame;
    [SerializeField] private CanvasGroup _transperancyTarget;

    private List<IUnitBindable> _bindableDependants = new();

    private Unit _owner;
    
    private float _untargetTransperancy;
    private float _untargetScale;
    private Vector3 _scaleVecor;
    private bool _hidden;

    public Unit Owner => _owner;

    public void SubscribeForOwnerBinding(IUnitBindable bindable) {
        _bindableDependants.Add(bindable);
        if (_owner != null) {
            bindable.BindUnit(_owner);
        }
    }

    private void Awake() {
        
        _untargetScale = Lobby.ActiveAccount.Data.Settings.Nameplates.UnselectedSizePercent/100;
        _scaleVecor = new Vector3(_untargetScale, _untargetScale, _untargetScale);
        _untargetTransperancy = Lobby.ActiveAccount.Data.Settings.Nameplates.UnselectedTransparencyPercent;
        BindToUnit(transform.parent.GetComponent<Unit>());
        //gameObject.transform. = _scaleVecor;
    }

    public void BindToUnit(Unit owner) {
        /*if (owner != null)
            throw new Exception("Unable to bind unit to already binded nameplate.");*/
        if (owner == null)
            throw new Exception("Unable to bind nameplate to <Null>.");
        _owner = owner;
        foreach (IUnitBindable bindable in _bindableDependants)
            bindable.BindUnit(owner);
    }

    private void Unselect() {
        _selectionFrame.SetActive(false);
        //gameObject.transform.localScale = _scaleVecor;
        _transperancyTarget.alpha = _untargetTransperancy;
    }

    public void OnSelectionChanged(Unit wasSelected, Unit toSelect) {
        if (_owner == wasSelected) {
            Unselect();
        }
        if (toSelect == _owner) {
            Select();
        }
    }

    public override void UpdateUIData() {
        if(_hidden == false)
            _transperancyTarget.alpha = _transperancyTarget.alpha = _selectionFrame.activeSelf ? _untargetTransperancy : 1;
        gameObject.transform.localScale = _scaleVecor;
    }

    public override void Hide() {
        _hidden = true;
        _transperancyTarget.alpha = 0;
    }

    public override void Show() {
        _hidden = false;
        _transperancyTarget.alpha = _selectionFrame.activeSelf? _untargetTransperancy : 1;
    }
    private void OnEnable() {
        Controller.SelectionChanged += OnSelectionChanged;
    }

    private void OnDisable() {
        Controller.SelectionChanged -= OnSelectionChanged;
    }

    private void LateUpdate() {
        transform.LookAt(Camera.main.transform.forward + transform.position);
    }

    private void Select() {
        _selectionFrame.SetActive(true);
        //gameObject.transform.localScale = Vector3.one;
        _transperancyTarget.alpha = 1;
    }
}
