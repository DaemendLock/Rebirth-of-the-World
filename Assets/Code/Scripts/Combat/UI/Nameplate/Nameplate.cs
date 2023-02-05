using UnityEngine;

public class Nameplate : MonoBehaviour {
    [SerializeField] private GameObject _selectionFrame;

    private Unit _owner;

    public Unit Owner => _owner;

    private void Awake() {
        _owner = transform.parent.GetComponent<Unit>();
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

    public void OnSelectionChanged(Unit wasSelected, Unit toSelect) {
        if(_owner == wasSelected) {
            Unselect();
        }
        if (toSelect == _owner) {
            Select();
        }
    }

    private void Select() {
        _selectionFrame.SetActive(true);
    }

    private void Unselect() {
        _selectionFrame.SetActive(false);
    }


}
