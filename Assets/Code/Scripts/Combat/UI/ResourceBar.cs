using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour {
    public Image fillImage;
    public TextMeshProUGUI text;

    public void SetValue(float cur, float max) {
        fillImage.fillAmount = cur / max;
        text.text = ((int)cur).ToString();
    }
}