using System.Collections.Generic;
using View.Lobby.ScenarionSelection;
using View.Lobby.Utils;

public sealed class TeamSelection : MenuElement
{
    //private Scenario prepareFor;
    public List<MemberCard> members;

    //public Scenario PrepareFor { get => prepareFor; set => prepareFor = value; }

    public void SetupSelection(int selectionAmmount = 8)
    {
        //for (int i = 0; i < members.Count; i++) {
        //    if (prepareFor.UnitsEngaged >= i) {
        //        members[i].enable = true;
        //        members[i].gameObject.SetActive(true);
        //        if (i < selectionAmmount) {
        //            members[i].controlable = true;
        //        }
        //        if (i < prepareFor.GetUnitsPreset().Length) {
        //            members[i].SetUnit(prepareFor.GetUnitsPreset()[i]);
        //        }
        //    } else {
        //        members[i].enable = false;
        //        members[i].gameObject.SetActive(false);
        //    }
        //}
    }

    public void StartActivity()
    {

        //if (prepareFor == null) {
        //    return;
        //}
        //List<UnitPreset> presets = new List<UnitPreset>();
        //foreach (MemberCard unit in members) {
        //    if (unit.enable && prepareFor.GetSpawnLocations().Length > presets.Count) {
        //        presets.Add(new UnitPreset(unit, prepareFor.GetSpawnLocations()[presets.Count]));
        //    } else {
        //        break;
        //    }
        //}
        //Loader.Instance.LoadCombatScenario(prepareFor, presets);
    }
}
