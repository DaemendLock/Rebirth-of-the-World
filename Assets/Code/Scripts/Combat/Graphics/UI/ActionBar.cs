using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {
    public List<ResourceBar> resourceBars;
    public List<SpellCard> spellCards = new List<SpellCard>();
    private Unit _currentUnit;

    private void OnEnable() {
        Controller.SelectionChanged += UpdateBars;
    }

    private void OnDisable() {
        Controller.SelectionChanged -= UpdateBars;
    }

    public void UpdateBars(Unit wasSelected, Unit selected) {
        if (selected == _currentUnit) { return; }
        foreach (SpellCard s in spellCards) {
            s.InsertAbility(selected.GetAbilityByIndex(s.spellNum));
        }
        _currentUnit = selected;
        resourceBars[0].fillImage.color = selected.LeftColor;
        resourceBars[1].fillImage.color = selected.RightColor;
    }

    private void Update() {
        if (_currentUnit == null)
            return;
        for (int i = 0; i < resourceBars.Count; i++) {
            resourceBars[i].SetValue(_currentUnit.GetResource(i), _currentUnit.GetResourceMax(i));
        }
        foreach (SpellCard s in spellCards) {
            s.UpdateCd();
        }
    }

    public void OnClick() {

    }
}