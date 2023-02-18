using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class UnitGraphics : MonoBehaviour {
    private Unit _attachedUnit;
    private Vector3 _approximateLocation;

    private bool _showCastbar;

    private void Start() {
        
    }

    private void BindToUnit(Unit unit) {
        if (_attachedUnit != null)
            return;
        _attachedUnit = unit;
        ForceMove(unit.Origin);
    }

    private void OnEnable() {
        _attachedUnit.Moved += ForceMove;
    }

    private void OnDisable() {
        _attachedUnit.Moved -= ForceMove;
    }

    // Update is called once per frame
    private void Update() {
        if (_attachedUnit.Moving) {
            gameObject.transform.position = _attachedUnit.Origin;
            Quaternion rot = Quaternion.LookRotation(_attachedUnit.Destination - _attachedUnit.Origin);
            float rotAc = 0;
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rot.eulerAngles.y, ref rotAc, Time.deltaTime);
            gameObject.transform.position = _attachedUnit.Origin;
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    private void ForceMove(Vector3 location) {
        _approximateLocation = location;
        gameObject.transform.position = location;
    }


}
