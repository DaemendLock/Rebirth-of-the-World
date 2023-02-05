using System;
using UnityEngine;
using UnityEngine.UI;

public class MemberCard : MonoBehaviour {
    public Image memberPicture;
    public UnitPreview Unit { get; private set; }
    [NonSerialized]
    public bool controlable = true;
    [NonSerialized]
    public bool enable = true;

    public void SetUnit(UnitPreview unit) {
        Unit = unit;
        memberPicture.sprite = unit.icon;
    }

}
