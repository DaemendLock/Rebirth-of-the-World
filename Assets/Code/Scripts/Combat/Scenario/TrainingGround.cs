using UnityEngine;

public class TrainingGround : Scenario {

    private GameObject _boardPrefab = Resources.Load("Prefabs/UI/DamageStats") as GameObject;

    private DamageMeter _board;

    public override void OnLoad() {
        _board = Instantiate(_boardPrefab, UI.Instance.transform.parent).GetComponent<DamageMeter>();
        foreach (Unit unit in RotW.FindUnitsInRadius(Vector3.zero, float.PositiveInfinity)) {
            if (unit.Team != Team.TEAM_ALLY) { continue; }
            _board.AddUnitToTrack(unit);
        }
    }
}
