using Adapters.Combat;
using Assets.Sources.Temp.Template;
using Core.Lobby.Encounters;
using System.Threading.Tasks;
using UnityEngine;
using Utils.DataTypes;
using View.Combat.Units;
using View.Lobby.TeamSetup;

namespace Assets.Sources.Temp
{
    internal class AutoStartButton : MonoBehaviour
    {
        [SerializeField] private Encounter _encounter;

        private void OnEnable()
        {
            View.Lobby.Lobby.Instance?.StartScenario(_encounter);
            TeamSetup.Start += StartCombat;
        }

        private void OnDisable()
        {
            TeamSetup.Start -= StartCombat;
        }

        public async void StartCombat(UnitCreationData[] eventData)
        {
            Networking.Combat.UseClient(new AutoAcceptClient());

            await Loader.LoadScene(1);
            await Task.Delay(1000);

            LoadChractersToCombat(eventData);
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
