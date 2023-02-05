using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberPreview : MonoBehaviour {
    public bool inSelectMode = false;
    public UnitPreview _unit;

    [Header("Components")]

    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private Image role;
    [SerializeField] private ProgressLvl level;
    [SerializeField] private ProgressLvl affection;
    [SerializeField] private List<Image> abilities;
    [SerializeField] private Animator showHideGear;


    //[SerializeField] private TextMeshPro insertButton;

    public void InsertUnit(UnitPreview unit) {
        _unit = unit;
        unitName.text = unit.name;
        level.SetValue(unit.lvl);
        affection.SetValue(unit.affection);
        for (int i = 0; i < abilities.Count; i++) {
            abilities[i].sprite = unit.baseData.abilities[i].icon;
        }
    }

    private void OnEnable() {
        /*if (insertButton == null) {
            insertButton.text = "Go to trial >";
            //REMOVE
            insertButton.gameObject.SetActive(false);
            return;
        }
        insertButton.text = "Go to trial >";
        //REMOVE
        insertButton.gameObject.SetActive(false);*/
    }

    public void ToggleGear() {
        showHideGear.SetBool("Show", !showHideGear.GetBool("Show") );
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
