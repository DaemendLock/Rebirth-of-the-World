using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    [Header("Abilities settings")]
    public KeyCode[] abilities = new KeyCode[8] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8 };
    public KeyCode stopCast = KeyCode.Escape;
    public float precastValue = 0;

    public static InputManager Instance { get; private set; }

    [Header("Targeting settings")]
    public KeyCode nextTarget = KeyCode.BackQuote;
    //public KeyCode keyCode = KeyCode.Tab;
    public KeyCode nextAlly;

    private void Start() {
        if (Instance == null)
            Instance = this;
    }

    private void Update() {
        if (Input.anyKey) {

            Unit control = Controller.Instance.SelectedUnit;

            Unit select = null;
            if (Input.anyKeyDown)
                select = Controller.GetCursorTarget();

            if (control != null) {
                for (int i = 0; i < abilities.Length; i++) {
                    if (Input.GetKeyDown(abilities[i])) {
                        if (control.Casting && control.CurrentCastAbility.CastTime <= precastValue) {
                            control.QueueAbility(control.GetAbilityByIndex(i));
                        }
                    }
                    if (Input.GetKeyDown(abilities[i])) {
                        control.CastAbility(control.GetAbilityByIndex(i));
                    }
                }
            }

            if (Input.GetKeyDown(nextAlly) && !Input.GetKey(KeyCode.LeftControl)) {
                Controller.Instance.ChooseNextAlly();
            }
        }
        /*
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            if (moving != null) {
                StopMoving();
            }
        }*/


    }

    public void SelectUnit(InputAction.CallbackContext context) {
        if (context.started) {
            Unit select = Controller.GetCursorTarget();
            if (select != null) {

            }
        }
    }

    public void NextAlly(InputAction.CallbackContext context) {


        //if (context.started) ChooseNextAlly();
    }

    public void PrevAlly(InputAction.CallbackContext context) {
        if (context.started)
            Controller.Instance.ChoosePrevAlly();
    }

    public void CancelCast(InputAction.CallbackContext context) {
        Controller.Instance.SelectedUnit?.Interrupt();
    }

    /*private void StopMoving() {
        if (moving== null) return;
        RaycastHit hit;
        if ( Physics.Raycast( Camera.main.ScreenPointToRay(GetCursorPosition()), out hit, Mathf.Infinity, layerMask: 3)) {
            moving.MoveToPosition(hit.point + moving.transform.position.y * Vector3.up);
            moving = null;
        }
    }*/

    public Vector3 GetCursorPosition() {
        return Input.mousePosition;
    }

    public string ToStringToken() {
        string res = abilities.ToString();
        return res;
    }

    public void FromStringToken() {

    }
}