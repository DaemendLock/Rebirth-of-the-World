using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyFrame : MonoBehaviour {

    private Unit unit;
    public UI globalUI;
    public Slider healhBar;
    public Slider resourceLeftBar;
    public Slider resourceRightBar;
    public GameObject statusBar ;
    
    public void SetUnit(Unit unit) {
        this.unit = unit;
    }

    void Update() {
        healhBar.value = unit.HealthPercent;
        List<Status> buff = unit.AllStatuses;
    }


}