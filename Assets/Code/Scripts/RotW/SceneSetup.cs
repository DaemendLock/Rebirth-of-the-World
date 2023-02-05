using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitPreset {

    [SerializeField] private string _name;
    [SerializeField] private SpawnLocation _transform;
    [SerializeField] private Team _team;
    [SerializeField] private bool _controlable;
    private Dictionary<GearSlot, Gear> gear;
    private ushort lvl;

    public UnitPreset(MemberCard unit, Transform transform) {
        _name = unit.Unit.name;
        _transform = new SpawnLocation(transform);
        _controlable = unit.controlable;
    }

    public UnitPreset(MemberCard unit, SpawnLocation transform) {
        _name = unit.Unit.name;
        _transform = transform;
        _controlable = unit.controlable;
    }

    public UnitPreset(string name, Vector3 location, Quaternion rotation, bool controlable = false) {
        _name = name;
        _transform = new SpawnLocation(location, rotation);
        _controlable = controlable;
    }

    public UnitPreset(string name, Vector3 location, Quaternion rotation, Team team, bool controlable = false) {
        _name = name;
        _transform = new SpawnLocation(location, rotation);
        this._team = team;
        _controlable = controlable;
    }

    public UnitPreset(string name, SpawnLocation transform, bool controlable = false) {
        _name = name;
        _transform = transform;
        _controlable = controlable;
    }

    public UnitPreset(string name, SpawnLocation transform, Team team, bool controlable = false) {
        _name = name;
        _transform = transform;
        this._team = team;
        _controlable = controlable;
    }

    public UnitPreset(string name, Transform transform, bool controlable = false) {
        _name = name;
        _transform = new SpawnLocation(transform);
        _controlable = controlable;
    }

    public UnitPreset(string name, Transform transform, Team team, bool controlable = false) {
        _name = name;
        _transform = new SpawnLocation(transform);
        this._team = team;
        _controlable = controlable;
    }

    public void Summon() {
        RotW.CreateUnitByName(_name, _transform, _team, _controlable);
    }

    public void Summon(Team team) {
        RotW.CreateUnitByName(_name, _transform, team, _controlable);
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
        if(enemy != null)
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


