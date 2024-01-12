using Adapters.Combat;
using Core.Combat.Engine;
using Core.Lobby.Encounters;
using Data.Characters;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils.DataTypes;
using View.Combat.Units;
using View.General;
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

            List<UnitCreationData> units = new();

            for (int i = 0; i < encounter.EncounterGroups.Length; i++)
            {
                foreach (Encounter.EncounterNpc unit in encounter.EncounterGroups[i].NPCs)
                {
                    units.Add(unit.GetUnitCreationData(units.Count, (byte) (i + encounter.PlayerCount)));
                }
            }

            int prevLen = units.Count;

            for (int i = 0; i < eventData.Length; i++)
            {
                units.Add(Character.Get(eventData[i].CharacterId).GetUnitCreationData(i + prevLen, 0, eventData[i], 0));
            }

            LoadChractersToCombat(units);
        }

        private void LoadChractersToCombat(IEnumerable<UnitCreationData> data)
        {
            foreach (UnitCreationData udata in data)
            {
                Combat.CreateUnit(udata.Id, udata.Model);
                UnitFactory.CreateUnit(udata);
                SelectionInfo.RegisterControlUnit(udata.Id, udata.ControlGroup);
            }
        }
    }
}
