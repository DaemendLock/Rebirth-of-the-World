using UnityEngine;
using UnityEngine.UI;
using View.Lobby.CharacterSheet;

namespace View.Lobby.Gallery
{
    public class GalleryCard : MonoBehaviour
    {
        public UnitPreview Unit;
        public Image image;

        public void SetUnit(UnitPreview unitPreview)
        {
            Unit = unitPreview;
            //image.sprite = unitPreview.baseData.GalleryCard;
        }
    }
}
