using UnityEngine;
using UnityEngine.UI;

public class GalleryCard : MonoBehaviour {
    public UnitPreview Unit;
    public Image image;

    public void SetUnit(UnitPreview unitPreview) {
        Unit = unitPreview;
        image.sprite = unitPreview.baseData.GalleryCard;
    }
}
