using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressLvl : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _progress;
    [SerializeField] private Image _pBar;
    public void SetValue(float val) {
        _progress.text = "Lv. " + ((int) val).ToString() + "/80";
        _pBar.fillAmount = val - (int) val;
    }
}
