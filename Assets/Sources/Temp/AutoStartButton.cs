using Adapters.Combat;
using Core.Lobby.Encounters;
using Data.Characters;
using System.Threading.Tasks;
using UnityEngine;
using Utils.DataTypes;
using View.Combat.Units;
using View.Lobby.TeamSetup;

namespace Assets.Sources.Temp
{
    internal class AutoStartButton : MonoBehaviour
    { 
        private void OnEnable()
        {
            TeamSetup.Start += StartCombat;
        }

        private void OnDisable()
        {
            TeamSetup.Start -= StartCombat;
        }

        public async void StartCombat(CharacterState[] eventData, Encounter encounter)
        {
            Networking.Combat.UseClient(new AutoAcceptClient());

            await Loader.LoadScene(1);
            await Task.Delay(1000);

            UnitCreationData[] units = new UnitCreationData[eventData.Length + encounter.EncounterUnits.Length];

            for (int i = 0; i < encounter.EncounterUnits.Length; i++)
            {
                units[i] = encounter.EncounterUnits[i].GetUnitCreationData(i);
            }

            for (int i = 0; i < eventData.Length; i++)
            {
                units[i + encounter.EncounterUnits.Length] = Character.Get(eventData[i].CharacterId).GetUnitCreationData(i + encounter.EncounterUnits.Length, 0, eventData[i]);
            }

            LoadChractersToCombat(units);
        }

        private void LoadChractersToCombat(UnitCreationData[] data)
        {
            foreach (UnitCreationData udata in data)
            {
                Core.Combat.Engine.Combat.CreateUnit(udata.Id, udata.Model);
                UnitFactory.CreateUnit(udata);
                SelectionInfo.RegisterControllUnit(udata.Id, udata.ControlGroup);
            }
        }
    }
}
