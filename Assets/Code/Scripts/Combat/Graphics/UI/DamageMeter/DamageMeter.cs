using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageMeter : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private GameObject _recountUnitPrefab;

    private Dictionary<Unit, RecountUnit> units;

    private void OnEnable() {
        EventManager.UnitAttacked += TrackAttacks;
    }

    private void OnDisable() {
        EventManager.UnitAttacked -= TrackAttacks;
    }

    public void AddUnitToTrack(Unit unit) {
        if (units.ContainsKey(unit)) {
            return;
        }
        RecountUnit newUnit = Instantiate(_recountUnitPrefab, gameObject.transform).GetComponent<RecountUnit>();
        units[unit] = newUnit;
    }


    private void TrackAttacks(AttackEventInstance e) {
        if (units.ContainsKey(e.Inflictor)) {
            units[e.Inflictor].UpdateDamage(e);
        }
    }

}
