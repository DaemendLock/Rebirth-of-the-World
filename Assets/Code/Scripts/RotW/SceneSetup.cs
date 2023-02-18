using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitPreset {

    [SerializeField] private string _name;
    [SerializeField] private SpawnLocation _spawnLocation;
    [SerializeField] private Team _team;
    [SerializeField] private bool _controlable;
    private UnitPreview _dataSource;

    public UnitPreset(MemberCard unit, Transform transform) => Init(unit.Unit, new SpawnLocation(transform), unit.controlable);

    public UnitPreset(MemberCard unit, SpawnLocation spawnLocation) => Init(unit.Unit, spawnLocation, unit.controlable);


    public UnitPreset(string name, Vector3 location, Quaternion rotation, bool controlable = false) {
        //Init(name, new SpawnLocation(location, rotation), controlable);
    }

    private void Init(UnitPreview unit, SpawnLocation location, bool controlable) {
        _name = unit.name;
        _spawnLocation = location;
        _controlable = controlable;
        _dataSource = unit;
    }


    public UnitPreset(string name, Vector3 location, Quaternion rotation, Team team, bool controlable = false) {
        _name = name;
        _spawnLocation = new SpawnLocation(location, rotation);
        _team = team;
        _controlable = controlable;
    }

    public UnitPreset(string name, SpawnLocation transform, bool controlable = false) {
        _name = name;
        _spawnLocation = transform;
        _controlable = controlable;
    }

    public UnitPreset(string name, SpawnLocation transform, Team team, bool controlable = false) {
        _name = name;
        _spawnLocation = transform;
        _team = team;
        _controlable = controlable;
    }

    public UnitPreset(string name, Transform transform, bool controlable = false) {
        _name = name;
        _spawnLocation = new SpawnLocation(transform);
        _controlable = controlable;
    }

    public UnitPreset(string name, Transform transform, Team team, bool controlable = false) {
        _name = name;
        _spawnLocation = new SpawnLocation(transform);
        _team = team;
        _controlable = controlable;
    }

    public void Summon() {
        Summon(_team);
    }

    public void Summon(Team team) {
        Unit buf = RotW.CreateUnitByName(_name, _spawnLocation, team, _controlable);
        buf.SetLevelAndAffinity((byte) _dataSource.lvl, (byte) _dataSource.affection);
        buf.EquipWithAll(_dataSource.GetGear());
    }
}

public class SceneSetup {
    private readonly Scenario bufferedScenario;
    private readonly List<UnitPreset> lastParty;

    public SceneSetup(Scenario bufferedScenario, List<UnitPreset> lastParty) {
        this.bufferedScenario = bufferedScenario;
        this.lastParty = lastParty;
    }

    public void SetupGame(GameObject terrain, UnitPreset[] allies, UnitPreset[] enemy) {
        Controller.Instance.Clear();
        UnityEngine.Object.Instantiate(terrain);
        if (allies != null)
            foreach (UnitPreset unit in allies) {
                unit.Summon(Team.TEAM_ALLY);
            }
        if (enemy != null)
            foreach (UnitPreset unit in enemy) {
                unit.Summon(Team.TEAM_ENEMY);
            }
    }

    public void SetupPartyUnits() {
        foreach (UnitPreset us in lastParty)
            us.Summon();
    }

    public Scenario BufferedScenario => bufferedScenario;

    public List<UnitPreset> LastParty => lastParty;


}


