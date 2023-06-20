using UnityEngine;

public class UnitGraphics : MonoBehaviour {
    private Unit _attachedUnit;
    private Vector3 _approximateLocation;

    private bool _showCastbar;

    private void Awake() {
        gameObject.SetActive(false);
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
        float time = Time.deltaTime;
        if (_attachedUnit.Moving) Move(time);
    }

    private void UnitDied() {
        transform.Rotate(90, 0, 0);
    }

    private void ForceMove(Vector3 location) {
        _approximateLocation = location;
        gameObject.transform.position = location;
    }

    private void Move(float time) {
        Quaternion rot = _attachedUnit.Facing;
        float rotAc = 0;
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rot.eulerAngles.y, ref rotAc, time);
        
        transform.position = _attachedUnit.Origin;
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }


}
