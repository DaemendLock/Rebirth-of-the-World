using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour{

    public Image iconObejct;

    [NonSerialized]
    public Sprite icon;
    [NonSerialized]
    public int UID;
    [NonSerialized]
    public bool ready;

    void Start() {

    }
}
