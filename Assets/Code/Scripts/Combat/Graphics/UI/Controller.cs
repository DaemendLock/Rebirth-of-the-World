using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class Controller : MonoBehaviour {

    private static Controller instacne;

    public static event Action<Unit, Unit> SelectionChanged;

    private Unit _currentSelection;
    private int _targetIndex;
    private int _allyIndex = -1;
    private readonly List<Unit> _controlUnits = new List<Unit>();
    public int aliveAlly = 1;
    private static RaycastHit hit;
    public static Unit[] units;

    public Unit SelectedUnit => _currentSelection;

    public static Controller Instance => instacne;

    private void Start() {
        instacne = this;
    }

    public void SetSelectedUnit(Unit unit) {
        if (unit.Team == Team.ALLY && _controlUnits.Contains(unit)) {
            SelectionChanged?.Invoke(_currentSelection, unit);
            _currentSelection = unit;
            _allyIndex = _controlUnits.IndexOf(unit);
        }
    }

    public static Unit GetCursorTarget() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.CompareTag("Nameplate")) {
                Debug.Log("Idi nahui");
                return hit.collider.gameObject.transform.parent.GetComponent<Unit>();
            } else if (hit.collider.gameObject.CompareTag("Unit")) {
                return hit.collider.gameObject.GetComponent<Unit>();
            } else {
                return null;
            }
        }
        return null;
    }

    public void ChoosePrevAlly() {
        if (aliveAlly == 0)
            return;
        _allyIndex--;
        if (_allyIndex < 0) {
            _allyIndex += _controlUnits.Count;
        }
        if (_controlUnits[_allyIndex].Dead) {
            ChoosePrevAlly();
            return;
        }
        SetSelectedUnit(_controlUnits[_allyIndex]);
    }

    public void ChooseNextAlly() {
        if (aliveAlly == 0)
            return;
        _allyIndex++;
        if (_allyIndex == _controlUnits.Count) {
            _allyIndex = 0;
        }
        if (_controlUnits[_allyIndex].Dead) {
            ChooseNextAlly();
            return;
        }

        SetSelectedUnit(_controlUnits[_allyIndex]);
    }

    public void AddSelectableUnit(Unit unit) {
        _controlUnits.Add(unit);
    }

    public void Clear() {
        _controlUnits.Clear();
        _currentSelection = null;
        aliveAlly = 0;
    }

    public static Vector3 GetCursorPostion() {
        return Vector3.zero;
    }

    public void HandleUnitInteraction(Unit unit, PointerEventData data) {
        switch (data.button) {
            case PointerEventData.InputButton.Left:
                SetSelectedUnit(unit);
                break;
            case PointerEventData.InputButton.Right:
                if (unit.Team == Team.ENEMY)
                    _currentSelection.StartAttack(unit);
                break;
            default:
                break;
        }
    }
}
