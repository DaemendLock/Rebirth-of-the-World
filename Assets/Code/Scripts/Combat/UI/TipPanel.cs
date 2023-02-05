using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TipPanel : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textPanel;

    public void OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
        EventManager.SendTipCloseEvent();
    }
}
